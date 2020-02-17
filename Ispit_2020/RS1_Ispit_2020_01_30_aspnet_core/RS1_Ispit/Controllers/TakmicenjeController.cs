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
    public class TakmicenjeController : Controller
    {
        private MojContext db;
        public TakmicenjeController(MojContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            TakmicenjeFormVM model = new TakmicenjeFormVM()
            {
                Skola = db.Skola.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult TakmicenjeIndex(TakmicenjeFormVM tempModel)
        {
            Skola skola = db.Skola.Find(tempModel.SkolaID);

            TakmicenjeIndexVM model = new TakmicenjeIndexVM()
            {
                SkolaID = tempModel.SkolaID,
                SkolaNaziv = skola.Naziv,
                Razred = tempModel.RazredID,
                Rows = db.Takmicenje.Where(x => x.SkolaID == tempModel.SkolaID).Select(x => new TakmicenjeIndexVM.Row()
                {
                    TakmicenjeID = x.Id,
                    Predmet = x.Predmet.Naziv,
                    Razred = x.Razred,
                    Datum = x.Datum,
                    BrojUcenikaNisuPristupili = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeID == x.Id && y.Pristupio == false).Count(),
                    NajboljaSkola = db.OdjeljenjeStavka.Where(y => y.Odjeljenje.SkolaID == x.SkolaID)
                                                            .Select(y => y.DodjeljenPredmets.OrderByDescending(d => d.ZakljucnoKrajGodine)
                                                            .Select(d => d.OdjeljenjeStavka.Odjeljenje.Skola.Naziv).FirstOrDefault()).FirstOrDefault(),

                    NajboljeOdjeljenje = db.OdjeljenjeStavka.Where(y => y.Odjeljenje.SkolaID == x.SkolaID)
                                                            .Select(y => y.DodjeljenPredmets.OrderByDescending(d => d.ZakljucnoKrajGodine)
                                                            .Select(d => d.OdjeljenjeStavka.Odjeljenje.Oznaka).FirstOrDefault()).FirstOrDefault(),

                    NajboljiUcesnik = db.OdjeljenjeStavka.Where(y => y.Odjeljenje.SkolaID == x.SkolaID)
                                                            .Select(y => y.DodjeljenPredmets.OrderByDescending(d => d.ZakljucnoKrajGodine)
                                                            .Select(d => d.OdjeljenjeStavka.Ucenik.ImePrezime).FirstOrDefault()).FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int SkolaID)
        {
            Skola skola = db.Skola.Find(SkolaID);

            TakmicenjeAddVM model = new TakmicenjeAddVM()
            {
                SkolaID = SkolaID,
                SkolaNaziv = skola.Naziv,
                Predmeti = db.Predmet.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Save(TakmicenjeAddVM model)
        {
            Takmicenje novoTakmicenje = new Takmicenje()
            {
                SkolaID = model.SkolaID,
                Razred = model.RazredID,
                PredmetID = model.PredmetID,
                Datum = model.Datum
            };
            db.Takmicenje.Add(novoTakmicenje);

            var predmeti = db.DodjeljenPredmet.Where(x => x.PredmetId == model.PredmetID && x.ZakljucnoKrajGodine == 5).ToList();

            var ucenici = db.OdjeljenjeStavka.Where(x => x.DodjeljenPredmets.Any(y => y.OdjeljenjeStavkaId == x.Id)).ToList();

            foreach (var u in ucenici)
            {
                var prosjek = db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == u.Id).Average(x => x.ZakljucnoKrajGodine);

                if (prosjek > 4)
                {
                    TakmicenjeUcesnik noviUcesnik = new TakmicenjeUcesnik()
                    {
                        TakmicenjeID = novoTakmicenje.Id,
                        OdjeljenjeStavkaID = u.Id
                    };
                    db.TakmicenjeUcesnik.Add(noviUcesnik);
                }
            }
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Rezultati(int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeID)
                                                 .Include(x=> x.Predmet)
                                                 .Include(x=> x.Skola)
                                                 .FirstOrDefault();

            RezultatiTakmicenjaVM model = new RezultatiTakmicenjaVM()
            {
                TakmicenjeID=TakmicenjeID,
                Skola = takmicenje.Skola.Naziv,
                Predmet = takmicenje.Predmet.Naziv,
                Razred = takmicenje.Razred,
                Datum = takmicenje.Datum
            };
            return View(model);
        }

        public IActionResult Zakljucaj(int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeID)
                                                .Include(x => x.Predmet)
                                                .Include(x => x.Skola)
                                                .FirstOrDefault();
            takmicenje.Zakljucano = true;
            db.Takmicenje.Update(takmicenje);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + takmicenje.Id);
        }

        public IActionResult TakmicenjeUcesnikIndex (int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeID)
                                               .Include(x => x.Predmet)
                                               .Include(x => x.Skola)
                                               .FirstOrDefault();

            TakmicenjeUcesnikIndexVM model = new TakmicenjeUcesnikIndexVM()
            {
                TakmicenjeID = TakmicenjeID,
                Rows = db.TakmicenjeUcesnik.Where(x => x.TakmicenjeID == TakmicenjeID)
                                           .Select(x => new TakmicenjeUcesnikIndexVM.Row()
                {
                    TakmicenjeUcesnikID = x.Id,
                    Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                    BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                    Pristupio = x.Pristupio ? "DA" : "NE",
                    Bodovi = x.Bodovi
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Pristupio(int TakmicenjeUcesnikID)
        {
            TakmicenjeUcesnik ucesnik = db.TakmicenjeUcesnik.Where(x => x.Id == TakmicenjeUcesnikID).FirstOrDefault();

            if (ucesnik.Pristupio == false)
            {
                ucesnik.Pristupio = true;
            }
            else
            {
                ucesnik.Pristupio = false;
            }
            db.TakmicenjeUcesnik.Update(ucesnik);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + ucesnik.TakmicenjeID);
        }

        public IActionResult Edit(int TakmicenjeUcesnikID)
        {
            TakmicenjeUcesnik ucesnik = db.TakmicenjeUcesnik.Where(x => x.Id == TakmicenjeUcesnikID)
                                                            .Include(x=> x.OdjeljenjeStavka)
                                                            .ThenInclude(x=> x.Ucenik)
                                                            .FirstOrDefault();

            TakmicenjeUcesnikEditVM model = new TakmicenjeUcesnikEditVM()
            {
                TakmicenjeUcesnikID = TakmicenjeUcesnikID,
                Ucesnik = ucesnik.OdjeljenjeStavka.Ucenik.ImePrezime
            };
            return PartialView(model);
        }

        public IActionResult SaveEdit(TakmicenjeUcesnikEditVM model)
        {
            TakmicenjeUcesnik ucesnik = db.TakmicenjeUcesnik.Where(x => x.Id == model.TakmicenjeUcesnikID)
                                                          .Include(x => x.OdjeljenjeStavka)
                                                          .ThenInclude(x => x.Ucenik)
                                                          .FirstOrDefault();
            ucesnik.Bodovi = model.Bodovi;
            db.TakmicenjeUcesnik.Update(ucesnik);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + ucesnik.TakmicenjeID);
        }

        public IActionResult UcesnikAdd(int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Find(TakmicenjeID);

            TakmicenjeUcesnikAddVM model = new TakmicenjeUcesnikAddVM()
            {
                TakmicenjeID = TakmicenjeID,
                Ucesnik = db.TakmicenjeUcesnik.Where(x => x.TakmicenjeID == TakmicenjeID).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.OdjeljenjeStavka.UcenikId.ToString(),
                    Text = x.OdjeljenjeStavka.Ucenik.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult UcesnikSave(TakmicenjeUcesnikAddVM model)
        {
            OdjeljenjeStavka stavka = db.OdjeljenjeStavka.Where(x => x.UcenikId == model.UcesnikID).FirstOrDefault();
            TakmicenjeUcesnik ucesnik = new TakmicenjeUcesnik()
            {
                TakmicenjeID = model.TakmicenjeID,
                OdjeljenjeStavkaID = stavka.Id,
                Bodovi=model.Bodovi
            };
            db.TakmicenjeUcesnik.Add(ucesnik);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + ucesnik.TakmicenjeID);
        }
    }
}