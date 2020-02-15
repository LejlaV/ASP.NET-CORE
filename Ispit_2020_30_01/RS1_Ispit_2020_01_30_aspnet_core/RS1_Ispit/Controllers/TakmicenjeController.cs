using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class TakmicenjeController : Controller
    {
        private MojContext db;
        public TakmicenjeController (MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            SkolaIndexVM model = new SkolaIndexVM
            {
                Skole = db.Skola.Select(x => new SelectListItem()
                {
                    Value=x.Id.ToString(),
                    Text=x.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult TakmicenjeIndex(SkolaIndexVM modelTemp)
        {
            Skola skola = db.Skola.Where(x => x.Id == modelTemp.SkolaID).FirstOrDefault();
            int razred = modelTemp.RazredID;

            TakmicenjeIndexVM model = new TakmicenjeIndexVM()
            {
                SkolaID = skola.Id,
                Skola = skola.Naziv,
                Razred = razred,
                Rows = db.Takmicenje.Where(x => x.SkolaId == skola.Id && x.Razred == razred).Select(x => new ViewModels.TakmicenjeIndexVM.Row()
                {
                    TakmicenjeID = x.Id,
                    Predmet = x.Predmet.Naziv,
                    Razred = razred,
                    Datum = x.Datum,
                    BrojUcesnikaNisuPristupili = db.TakmicenjeUcesnik.Where(y => y.TakmicenjeID == x.Id && y.Pristupio==false).Count(),
                    NajboljaSkola = db.OdjeljenjeStavka.Where(y=> y.Odjeljenje.SkolaID==skola.Id)
                                                       .Select(y=> y.DodjeljenPredmets
                                                       .OrderByDescending(p=> p.ZakljucnoKrajGodine)
                                                       .Select(p=> p.OdjeljenjeStavka.Odjeljenje.Skola.Naziv)
                                                       .FirstOrDefault()).FirstOrDefault(),
                    NajboljeOdjeljenje = db.OdjeljenjeStavka.Where(y => y.Odjeljenje.SkolaID == skola.Id)
                                                       .Select(y => y.DodjeljenPredmets
                                                       .OrderByDescending(p => p.ZakljucnoKrajGodine)
                                                       .Select(p => p.OdjeljenjeStavka.Odjeljenje.Oznaka)
                                                       .FirstOrDefault()).FirstOrDefault(),
                    NajboljiUcesnik = db.OdjeljenjeStavka.Where(y => y.Odjeljenje.SkolaID == skola.Id)
                                                       .Select(y => y.DodjeljenPredmets
                                                       .OrderByDescending(p => p.ZakljucnoKrajGodine)
                                                       .Select(p => p.OdjeljenjeStavka.Ucenik.ImePrezime)
                                                       .FirstOrDefault()).FirstOrDefault(),
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Add(int SkolaID)
        {
            Skola skola = db.Skola.Where(x => x.Id == SkolaID).FirstOrDefault();

            TakmicenjeAddVM model = new TakmicenjeAddVM()
            {
                SkolaID=skola.Id,
                Skola=skola.Naziv,
                
                Predmeti = db.Predmet
                .Select(x=> new SelectListItem()
                {
                    Value=x.Id.ToString(),
                    Text= x.Naziv
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Save(TakmicenjeAddVM model)
        {
            Takmicenje novoTakmicenje = new Takmicenje()
            {
                SkolaId=model.SkolaID,
                PredmetID=model.PredmetID,
                Razred=model.RazredID,
                Datum=model.Datum
            };

            db.Takmicenje.Add(novoTakmicenje);

            var predmeti = db.DodjeljenPredmet.Where(x => x.ZakljucnoKrajGodine == 5).ToList();
            var ucenici = db.OdjeljenjeStavka.Where(x => x.Odjeljenje.SkolaID == model.SkolaID && predmeti.Any(y => y.OdjeljenjeStavkaId == x.Id)).ToList();

            foreach (var uc in ucenici)
            {
                double prosjek = db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == uc.Id).Average(x => (int?)x.ZakljucnoKrajGodine ?? 0);

                if (prosjek > 4)
                {
                    TakmicenjeUcesnik noviUcesnik = new TakmicenjeUcesnik()
                    {
                        TakmicenjeID = novoTakmicenje.Id,
                        OdjeljenjeStavkaID = uc.Id,
                        Pristupio = false,
                        Bodovi = 0
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
                                                 .Include(x=> x.Skola).FirstOrDefault();

            TakmicenjeRezultatiVM model = new TakmicenjeRezultatiVM()
            {
                TakmicenjeID=TakmicenjeID,
                Skola=takmicenje.Skola.Naziv,
                Predmet=takmicenje.Predmet.Naziv,
                Razred=takmicenje.Razred,
                Datum=takmicenje.Datum
            };

            return View(model);
        }

        public IActionResult TakmicenjeUcesnikIndex(int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeID)
                                                 .Include(x => x.Predmet)
                                                 .Include(x => x.Skola).FirstOrDefault();

            TakmicenjeUcesnikIndexVM model = new TakmicenjeUcesnikIndexVM()
            {
                TakmicenjeID = TakmicenjeID,
                Rows = db.TakmicenjeUcesnik.Where(x => x.TakmicenjeID == TakmicenjeID).Select(x => new TakmicenjeUcesnikIndexVM.Row()
                {
                    TakmicenjeUcesnikID = x.Id,
                    Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                    BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                    Pristupio = x.Pristupio ? "DA" : "NE",
                    Rezultat = x.Bodovi
                }).ToList()
            };

            return PartialView(model);
        }

        public IActionResult Zakljucaj(int TakmicenjeID)
        {
            Takmicenje takmicenje = db.Takmicenje.Where(x => x.Id == TakmicenjeID)
                                                .Include(x => x.Predmet)
                                                .Include(x => x.Skola).FirstOrDefault();

            takmicenje.Zakljucano = true;
            db.Takmicenje.Update(takmicenje);
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + takmicenje.Id);
        }
        public IActionResult Pristupio(int TakmicenjeUcesnikID)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Where(x => x.Id == TakmicenjeUcesnikID).FirstOrDefault();

            if (takmicenjeUcesnik.Pristupio == true)
            {
                takmicenjeUcesnik.Pristupio = false;
            }
            else
            {
                takmicenjeUcesnik.Pristupio = true;
            }

            db.TakmicenjeUcesnik.Update(takmicenjeUcesnik);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + takmicenjeUcesnik.TakmicenjeID);
        }

        public IActionResult TakmicenjeUcesnikUpdate(int TakmicenjeUcesnikID)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Where(x => x.Id == TakmicenjeUcesnikID).FirstOrDefault();

            TakmicenjeUcesnikUpdateVM model = new TakmicenjeUcesnikUpdateVM()
            {
                TakmicenjeUcesnikID=TakmicenjeUcesnikID,
                Ucesnici= db.OdjeljenjeStavka.Where(x=> x.Id==takmicenjeUcesnik.OdjeljenjeStavkaID).Select(x=> new SelectListItem()
                {
                    Value=x.Ucenik.Id.ToString(),
                    Text=x.Ucenik.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult TakmicenjeUcesnikSave(TakmicenjeUcesnikUpdateVM model)
        {
            TakmicenjeUcesnik takmicenjeUcesnik = db.TakmicenjeUcesnik.Where(x => x.Id == model.TakmicenjeUcesnikID).FirstOrDefault();

            takmicenjeUcesnik.Bodovi = model.Bodovi;
            db.TakmicenjeUcesnik.Update(takmicenjeUcesnik);
            db.SaveChanges();
            return Redirect("/Takmicenje/Rezultati?TakmicenjeID=" + takmicenjeUcesnik.TakmicenjeID);
        }
    }
}