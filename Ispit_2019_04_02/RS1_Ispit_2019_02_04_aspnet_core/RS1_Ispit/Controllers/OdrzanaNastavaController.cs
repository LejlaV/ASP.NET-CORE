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
            OdrzanaNastavaIndexVM model = new OdrzanaNastavaIndexVM()
            {
                Rows = db.Nastavnik.Select(x => new OdrzanaNastavaIndexVM.Row()
                {
                    NastavnikID = x.Id,
                    ImePrezime = x.Ime + " " + x.Prezime,
                    BrojCasova = db.OdrzaniCasDetalji.Where(y => y.OdrzaniCas.PredajePredmet.NastavnikID == x.Id).Count()
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
                Rows = db.OdrzaniCas.Select(x => new OdrzanaNastavaOdaberiVM.Row()
                {
                    OdrzaniCasID = x.Id,
                    Datum = x.Datum,
                    Skola = x.PredajePredmet.Odjeljenje.Skola.Naziv,
                    SkGodOdjeljenje = x.PredajePredmet.Odjeljenje.SkolskaGodina.Naziv + " " + x.PredajePredmet.Odjeljenje.Oznaka,
                    Predmet = x.PredajePredmet.Predmet.Naziv,
                    OdsutniUcenici = db.OdrzaniCasDetalji.Where(y => y.OdrzaniCasID == x.Id && y.Pristuan == false)
                      .Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Find(NastavnikID);

            OdrzaniCasAddVM model = new OdrzaniCasAddVM()
            {
                NastavnikID = NastavnikID,
                ImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                PredajePredmet = db.PredajePredmet.Where(x => x.NastavnikID == NastavnikID)
                                .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                                {
                                    Value = x.Id.ToString(),
                                    Text = $"{x.Odjeljenje.Skola.Naziv}/{x.Odjeljenje.Oznaka}/{x.Predmet.Naziv}"
                                }).ToList()
            };

            return View(model);
        }

        public IActionResult Save(OdrzaniCasAddVM model)
        {
            OdrzaniCas noviOdrzaniCas = new OdrzaniCas();
            noviOdrzaniCas.Datum = model.Datum;
            noviOdrzaniCas.Sadrzaj = model.Sadrzaj;
            noviOdrzaniCas.PredajePredmetID = model.PredajePredmetID;

            db.OdrzaniCas.Add(noviOdrzaniCas);

            if (model != null)
            {
                var predajePredmet = db.PredajePredmet.Where(x => x.Id == model.PredajePredmetID).FirstOrDefault();

                    var ucenici = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == predajePredmet.OdjeljenjeID).ToList();

                    foreach (var uc in ucenici)
                    {
                        if (predajePredmet.OdjeljenjeID == uc.OdjeljenjeId)
                        {
                            OdrzaniCasDetalji noviDetalji = new OdrzaniCasDetalji()
                            {
                                Pristuan = false,
                                Ocjena = 0,
                                OpravdanoOdsutan = false,
                                OdrzaniCasID = noviOdrzaniCas.Id,
                                OdjeljenjeStavkaID = uc.Id
                            };

                            db.OdrzaniCasDetalji.Add(noviDetalji);
                        }
                    }
            }
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Odaberi?NastavnikID=" + model.NastavnikID);
        }

        public IActionResult Detalji(int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x=> x.Id==OdrzaniCasID)
                                                 .Include(x=> x.PredajePredmet)
                                                 .Include(x => x.PredajePredmet.Predmet)
                                                 .Include(x => x.PredajePredmet.Odjeljenje)
                                                 .Include(x => x.PredajePredmet.Odjeljenje.Skola)
                                                 .FirstOrDefault();
            OdrzanaNastavaDetaljiVM model = new OdrzanaNastavaDetaljiVM()
            {
                OdrzaniCasID = odrzaniCas.Id,
                PredajePredmet = odrzaniCas.PredajePredmet.Odjeljenje.Skola.Naziv + "/" +
                               odrzaniCas.PredajePredmet.Odjeljenje.Oznaka + "/" + odrzaniCas.PredajePredmet.Predmet.Naziv,
                Sadrzaj = odrzaniCas.Sadrzaj
            };
            return View(model);
        }

        public IActionResult Obrisi(int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasID)
                                                 .Include(x => x.PredajePredmet)
                                                 .Include(x => x.PredajePredmet.Predmet)
                                                 .Include(x => x.PredajePredmet.Odjeljenje)
                                                 .Include(x => x.PredajePredmet.Odjeljenje.Skola)
                                                 .FirstOrDefault();
            db.OdrzaniCas.Remove(odrzaniCas);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Odaberi?NastavnikID=" + odrzaniCas.PredajePredmet.NastavnikID);
        }
    }
}