using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;
using System.Linq;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        private MojContext db;

        public OdrzanaNastavaController (MojContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            NastavnikIndexVM model = new NastavnikIndexVM()
            {
                Rows = db.Nastavnik.Select(x => new NastavnikIndexVM.Row()
                {
                    NastavnikID = x.Id,
                    Skola = db.PredajePredmet.Where(y => y.NastavnikID == x.Id).Select(y => y.Odjeljenje.Skola.Naziv).FirstOrDefault(),
                    ImePrezime = x.Ime + " " + x.Prezime
                }).ToList()
            };
            return View(model);
        }

        public IActionResult MaturskiIspitIndex(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Where(x => x.Id == NastavnikID).FirstOrDefault();

            MaturskiIspitIndexVM model = new MaturskiIspitIndexVM()
            {
                NastavnikID = NastavnikID,
                NastavnikImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                Rows = db.MaturskiIspit.Select(x => new MaturskiIspitIndexVM.Row()
                {
                    MaturskiIspitID = x.Id,
                    Datum = x.Datum,
                    Skola = x.Skola.Naziv,
                    Predmet = x.Predmet.Naziv,
                    Ucenici = db.MaturskiIspitStavke.Where(y => y.MaturskiIspitID == x.Id).Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Where(x => x.Id == NastavnikID).FirstOrDefault();

            MaturskiIspitAddVM model = new MaturskiIspitAddVM()
            {
                NastavnikID = NastavnikID,
                NastavnikImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                SkolskaGodina = db.PredajePredmet.Where(x => x.NastavnikID == NastavnikID).Select(x => x.Odjeljenje.SkolskaGodina.Naziv).FirstOrDefault(),
                Skola = db.Skola.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList(),
                Predmet = db.PredajePredmet.Where(x=> x.NastavnikID==NastavnikID)
                                           .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Predmet.Id.ToString(),
                    Text = x.Predmet.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Save(MaturskiIspitAddVM model)
        {
            MaturskiIspit noviMaturskiIspit = new MaturskiIspit()
            {
                NastavnikID = model.NastavnikID,
                SkolaID = model.SkolaID,
                PredmetID = model.PredmetID,
                Datum = model.Datum
            };
            db.MaturskiIspit.Add(noviMaturskiIspit);

            var odjeljenje = db.Odjeljenje.Where(x => x.SkolaID == model.SkolaID && x.Razred == 4).ToList();

            var ucenici = db.OdjeljenjeStavka.Where(x => odjeljenje.Any(a => a.Id == x.OdjeljenjeId)).ToList();

            foreach (var u in ucenici)
            {
                if(db.DodjeljenPredmet.Where(x=> x.OdjeljenjeStavkaId==u.Id && x.ZakljucnoKrajGodine==1).Count()==0
                    || db.MaturskiIspitStavke.Where(x=> x.OdjeljenjeStavkaID==u.Id).Count(x=> x.Bodovi < 55) != 0)
                {
                    MaturskiIspitStavke noveStavke = new MaturskiIspitStavke()
                    {
                        MaturskiIspitID = noviMaturskiIspit.Id,
                        OdjeljenjeStavkaID = u.Id,
                        Bodovi = 0
                    };
                    db.MaturskiIspitStavke.Add(noveStavke);
                }
            }

            db.SaveChanges();
            return Redirect("/OdrzanaNastava/MaturskiIspitIndex?NastavnikID=" + noviMaturskiIspit.NastavnikID);
        }

        public IActionResult Edit(int MaturskiIspitID)
        {
            MaturskiIspit ispit = db.MaturskiIspit.Where(x => x.Id == MaturskiIspitID)
                                                  .Include(x=> x.Predmet)
                                                  .FirstOrDefault();

            MaturskiIspitEditVM model = new MaturskiIspitEditVM()
            {
                MaturskiIspitID = MaturskiIspitID,
                Datum=ispit.Datum,
                Predmet = ispit.Predmet.Naziv
            };
            return View(model);
        }

        public IActionResult SaveEdit(MaturskiIspitEditVM model)
        {
            MaturskiIspit ispit = db.MaturskiIspit.Where(x => x.Id == model.MaturskiIspitID)
                                            .Include(x => x.Predmet)
                                            .Include(x => x.Nastavnik)
                                            .FirstOrDefault();

            ispit.Napomena = model.Napomena;
            db.MaturskiIspit.Update(ispit);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/MaturskiIspitIndex?NastavnikID=" + ispit.NastavnikID);
        }

        public IActionResult MaturskiIspitStavkeIndex(int MaturskiIspitID)
        {
            MaturskiIspit ispit = db.MaturskiIspit.Where(x => x.Id == MaturskiIspitID)
                                            .Include(x => x.Predmet)
                                            .Include(x => x.Nastavnik)
                                            .FirstOrDefault();

            MaturskiIspitStavkeIndexVM model = new MaturskiIspitStavkeIndexVM()
            {
                MaturskiIspitID = MaturskiIspitID,
                Rows = db.MaturskiIspitStavke.Where(x => x.MaturskiIspitID == MaturskiIspitID)
                                             .Select(x => new MaturskiIspitStavkeIndexVM.Row()
                                             {
                                                 MaturskiIspitStavkeID = x.Id,
                                                 Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                                                 ProsjekOcjena = db.DodjeljenPredmet.Where(y => y.OdjeljenjeStavkaId == x.OdjeljenjeStavkaID).Average(y => y.ZakljucnoKrajGodine),
                                                 PristupIspitu = x.Pristupio ? "DA" : "NE",
                                                 Bodovi = x.Bodovi
                                             }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult StavkeEdit(int MaturskiIspitStavkeID)
        {
            MaturskiIspitStavke stavke = db.MaturskiIspitStavke.Where(x => x.Id == MaturskiIspitStavkeID)
                                                               .Include(x=> x.OdjeljenjeStavka)
                                                               .ThenInclude(x=> x.Ucenik)
                                                               .FirstOrDefault();

            MaturskiIspitStavkeEditVM model = new MaturskiIspitStavkeEditVM()
            {
                MaturskiIspitStavkeID = MaturskiIspitStavkeID,
                Ucenik = stavke.OdjeljenjeStavka.Ucenik.ImePrezime
            };
            return PartialView(model);
        }

        public IActionResult StavkeEditSave(MaturskiIspitStavkeEditVM model)
        {
            MaturskiIspitStavke stavke = db.MaturskiIspitStavke.Where(x => x.Id == model.MaturskiIspitStavkeID)
                                                               .Include(x=> x.MaturskiIspit)
                                                               .Include(x => x.OdjeljenjeStavka)
                                                               .ThenInclude(x => x.Ucenik)
                                                               .FirstOrDefault();
            stavke.Bodovi = model.Bodovi;
            db.MaturskiIspitStavke.Update(stavke);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Edit?MaturskiIspitID=" + stavke.MaturskiIspitID);
        }

        public IActionResult Pristup(int MaturskiIspitStavkeID)
        {
            MaturskiIspitStavke stavke = db.MaturskiIspitStavke.Where(x => x.Id == MaturskiIspitStavkeID)
                                                              .Include(x => x.MaturskiIspit)
                                                              .Include(x => x.OdjeljenjeStavka)
                                                              .ThenInclude(x => x.Ucenik)
                                                              .FirstOrDefault();
            if (stavke.Pristupio == true)
            {
                stavke.Pristupio = false;
            }
            else
            {
                stavke.Pristupio = true;
            }
            db.MaturskiIspitStavke.Update(stavke);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Edit?MaturskiIspitID=" + stavke.MaturskiIspitID);
        }
    }
}