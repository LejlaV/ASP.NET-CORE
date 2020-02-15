using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        private MojContext db;
        public OdrzanaNastavaController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            // prilikom uzimanja SkolaID za nastavnika treba ici preko tabele PredajePredmet iz razloga sto
            // sto su tu SVI nastavnici koji predaju nesto, dok u tabeli Odjeljenje su pohranjeni nastavnici
            // koji su samo ustvari razrednici nekom odjeljenju

            // model treba ispisati 30 zapisa jer je toliko nastavnika u bazi
            OdrzanaNastavaIndexVM model = new OdrzanaNastavaIndexVM()
            {
                Rows = db.Nastavnik.Select(x => new OdrzanaNastavaIndexVM.Row()
                {
                    NastavnikID=x.Id,
                    ImePrezime=x.Ime+" "+x.Prezime,
                    Skola=db.PredajePredmet.Where(y=> y.NastavnikID==x.Id).Select(y=> y.Odjeljenje.Skola.Naziv).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Odaberi(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Find(NastavnikID);
            OdrzanaNastavaOdaberiVM model = new OdrzanaNastavaOdaberiVM()
            {
                NastavnikID = NastavnikID,
                NastavnikImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                Rows = db.MaturskiIspit.Select(x => new OdrzanaNastavaOdaberiVM.Row()
                {
                    MaturskiIspitID = x.Id,
                    Datum=x.Datum,
                    Skola=x.Skola.Naziv,
                    Predmet=x.Predmet.Naziv,
                    UceniciNisuPristupili="Nisu pristupili"
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Add(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Find(NastavnikID);
            MaturskiIspitAddVM model = new MaturskiIspitAddVM()
            {
                NastavnikID = NastavnikID,
                NastavnikImePrezime = nastavnik.Ime+" "+nastavnik.Prezime,
                SkolskaGodina=db.PredajePredmet.Where(x=> x.NastavnikID==NastavnikID).Select(x=> x.Odjeljenje.SkolskaGodina.Naziv).FirstOrDefault(),
                
                Predmet = db.PredajePredmet.Where(x=> x.NastavnikID==NastavnikID)
                .Select(x=> new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value= x.Predmet.Id.ToString(),
                    Text=x.Predmet.Naziv
                }).ToList(),

                Skola = db.Skola.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Save(MaturskiIspitAddVM model)
        {
            MaturskiIspit noviMaturskiIspit = new MaturskiIspit();
            noviMaturskiIspit.Datum = model.Datum;
            noviMaturskiIspit.NastavnikID = model.NastavnikID;
            noviMaturskiIspit.SkolaID = model.SkolaID;
            noviMaturskiIspit.PredmetID = model.PredmetID;
            db.MaturskiIspit.Add(noviMaturskiIspit);

            // uslovi: Evidentiranje bodova SVIM Ucenicima IV razreda
            // Dodati zapise u tabelu MaturskiIspitStavke za maturante (IV razred) koji imaju uslov:
            // pozitivan uspjeh u IV razredu tj. zakljucna ocjena nije 1
            // broj bodova na prethodnom maturskom mora biti veci od 55

            if (model != null)
            {
                // u ovoj listi su svi ucenici IV razreda za odabranu skolu (Odabrana skola -> ID je u modelu)
                var uceniciIVRazreda = db.OdjeljenjeStavka.Where(x => x.Odjeljenje.SkolaID == model.SkolaID && x.Odjeljenje.Razred==4).ToList();
                
                // u ovoj listi su svi ucenici koju su na zadnjem evidentiranom imali vise od 55 bodova
                var uceniciPolozili = db.MaturskiIspitStavke.Where(x=> x.BrojBodova>55).Select(x => x.OdjeljenjeStavkaID).ToList();
                foreach (var ucenici in uceniciIVRazreda)
                {
                    // preko tabele dodjeljeniPredmet se provjrava OdjeljenjeStavkaID i provjerava se da li je isti predmet
                    // tj poredi se PredmetID sa odabranim predmetom (ID iz modela)
                    // moramo OdjeljenjeStavkaID provjeriti i sa listom uceniciPolozili

                   if(db.DodjeljenPredmet.Where(x=> x.OdjeljenjeStavkaId==ucenici.Id && x.PredmetId==model.PredmetID
                        && x.ZakljucnoKrajGodine!=1 && !uceniciPolozili.Contains(x.OdjeljenjeStavkaId)).Any())
                    {
                        db.MaturskiIspitStavke.Add(new MaturskiIspitStavke()
                        {
                            MaturskiIspitID = noviMaturskiIspit.Id,
                            OdjeljenjeStavkaID = ucenici.Id,
                            BrojBodova = null,
                            OdobrenPristup = true
                        });
                    }
                }
            }
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Odaberi?NastavnikID=" + model.NastavnikID);
        }

        public IActionResult Edit(int MaturskiIspitID)
        {
            MaturskiIspit maturskiIspit = db.MaturskiIspit.Where(x => x.Id == MaturskiIspitID)
                                                          .Include(x => x.Predmet)
                                                          .FirstOrDefault();
            MaturskiIspitEditVM model = new MaturskiIspitEditVM()
            {
                MaturskiIspitID = maturskiIspit.Id,
                NastavnikID = maturskiIspit.NastavnikID,
                Predmet = maturskiIspit.Predmet.Naziv,
                Napomena = maturskiIspit.Napomena
            };
            return View(model);
        }

        public IActionResult SaveEdit(MaturskiIspitEditVM model)
        {
            MaturskiIspit maturskiIspit = db.MaturskiIspit.Find(model.MaturskiIspitID);
            maturskiIspit.Napomena = model.Napomena;
            db.SaveChanges();
         
            return Redirect("/OdrzanaNastava/Odaberi?NastavnikID=" + model.NastavnikID);
        }
    }
}