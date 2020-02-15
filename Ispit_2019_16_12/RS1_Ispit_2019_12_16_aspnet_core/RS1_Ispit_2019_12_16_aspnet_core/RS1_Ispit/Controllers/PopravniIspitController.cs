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
    public class PopravniIspitController : Controller
    {
        private MojContext db;
        public PopravniIspitController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            IndexVM model = new IndexVM()
            {
                Skola = db.Skola.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList(),

                SkolskaGodina = db.SkolskaGodina.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList(),

                Predmet = db.Predmet.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult PopravniIspitIndex(IndexVM tempModel)
        {
            SkolskaGodina skolskaGodina = db.SkolskaGodina.Where(x => x.Id == tempModel.SkolskaGodinaID).FirstOrDefault();
        
            Skola skola = db.Skola.Where(x => x.Id == tempModel.SkolaID).FirstOrDefault();

            Predmet predmet = db.Predmet.Where(x => x.Id == tempModel.PredmetID).FirstOrDefault();

            PopravniIspitIndexVM model = new PopravniIspitIndexVM()
            {
                PredmetID=tempModel.PredmetID,
                Predmet = predmet.Naziv,
                SkolaID=tempModel.SkolaID,
                Skola = skola.Naziv,
                SkolskaGodinaID=tempModel.SkolskaGodinaID,
                SkolskaGodina = skolskaGodina.Naziv,
                Rows = db.PopravniIspit.Where(x => x.SkolskaGodinaID == tempModel.SkolskaGodinaID && x.SkolaID == tempModel.SkolaID && x.PredmetID == tempModel.PredmetID)
                                       .Select(x => new PopravniIspitIndexVM.Row()
                                       {
                                           PopravniIspitID = x.Id,
                                           Datum = x.Datum,
                                           Predmet = x.Predmet.Naziv,
                                           BrojUcenika = db.PopravniIspitStavke.Where(y => y.PopravniIspitID == x.Id && y.Pristup == true).Count(),
                                           BrojUcenikaPolozeno = db.PopravniIspitStavke.Where(y => y.PopravniIspitID == x.Id && y.Bodovi > 50).Count()
                                       }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int SkolaID, int SkolskaGodinaID, int PredmetID)
        {
            SkolskaGodina skolskaGodina = db.SkolskaGodina.Where(x => x.Id == SkolskaGodinaID).FirstOrDefault();

            Skola skola = db.Skola.Where(x => x.Id == SkolaID).FirstOrDefault();

            Predmet predmet = db.Predmet.Where(x => x.Id == PredmetID).FirstOrDefault();

            PopravniIspitAddVM model = new PopravniIspitAddVM()
            {
                SkolaID = SkolaID,
                Skola = skola.Naziv,
                SkolskaGodinaID = SkolskaGodinaID,
                SkolskaGodina = skolskaGodina.Naziv,
                PredmetID = PredmetID,
                Predmet = predmet.Naziv,

                Nastavnik1 = db.Nastavnik.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Ime + " " + x.Prezime
                }).ToList(),

                Nastavnik2 = db.Nastavnik.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Ime + " " + x.Prezime
                }).ToList(),

                Nastavnik3 = db.Nastavnik.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Ime + " " + x.Prezime
                }).ToList(),
            };
            return View(model);
        }

        public IActionResult Save(PopravniIspitAddVM model)
        {
            PopravniIspit noviPopravniIspit = new PopravniIspit()
            {
                Nastavnik1ID = model.Nastavnik1ID,
                Nastavnik2ID = model.Nastavnik2ID,
                Nastavnik3ID = model.Nastavnik3ID,
                SkolaID = model.SkolaID,
                SkolskaGodinaID = model.SkolskaGodinaID,
                PredmetID = model.PredmetID,
                Datum=model.Datum
            };
            db.PopravniIspit.Add(noviPopravniIspit);

            var ucenici = db.OdjeljenjeStavka.Where(x => x.Odjeljenje.SkolaID == model.SkolaID).ToList();

            PopravniIspitStavke noveStavke;
            foreach (var u in ucenici)
            {
                if(db.DodjeljenPredmet.Where(x=> x.OdjeljenjeStavkaId==u.Id && x.ZakljucnoKrajGodine == 1).Count() >= 3)
                {
                    noveStavke = new PopravniIspitStavke()
                    {
                        PopravniIspitID = noviPopravniIspit.Id,
                        OdjeljenjeStavkaID = u.Id,
                        Pristup = false,
                        Bodovi = 0,
                    };
                    db.PopravniIspitStavke.Add(noveStavke);
                }
                else if (db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == u.Id && x.PredmetId==model.PredmetID && x.ZakljucnoKrajGodine == 1).Count()>=1)
                {
                    noveStavke = new PopravniIspitStavke()
                    {
                        PopravniIspitID = noviPopravniIspit.Id,
                        OdjeljenjeStavkaID = u.Id,
                        Pristup = true,
                        Bodovi = null,
                    };
                    db.PopravniIspitStavke.Add(noveStavke);
                }
            }
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Detalji(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Where(x => x.Id == PopravniIspitID)
                                                          .Include(x=> x.Nastavnik1)
                                                          .Include(x=> x.Nastavnik2)
                                                          .Include(x=> x.Nastavnik3)
                                                          .Include(x=> x.Skola)
                                                          .Include(x=> x.SkolskaGodina)
                                                          .Include(x=> x.Predmet)
                                                          .FirstOrDefault();
            PopravniIspitDetaljiVM model = new PopravniIspitDetaljiVM()
            {
                PopravniIspitID = PopravniIspitID,
                Nastavnik1 = popravniIspit.Nastavnik1.Ime + " " + popravniIspit.Nastavnik1.Prezime,
                Nastavnik2 = popravniIspit.Nastavnik2.Ime + " " + popravniIspit.Nastavnik2.Prezime,
                Nastavnik3 = popravniIspit.Nastavnik3.Ime + " " + popravniIspit.Nastavnik3.Prezime,
                SkolskaGodina = popravniIspit.SkolskaGodina.Naziv,
                Skola = popravniIspit.Skola.Naziv,
                Predmet = popravniIspit.Predmet.Naziv,
                Datum = popravniIspit.Datum
            };
            return View(model);
        }

        public IActionResult PopravniIspitStavkeIndex(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Where(x => x.Id == PopravniIspitID)
                                                          .Include(x => x.Nastavnik1)
                                                          .Include(x => x.Nastavnik2)
                                                          .Include(x => x.Nastavnik3)
                                                          .Include(x => x.Skola)
                                                          .Include(x => x.SkolskaGodina)
                                                          .Include(x => x.Predmet)
                                                          .FirstOrDefault();

            PopravniIspitStavkeIndexVM model = new PopravniIspitStavkeIndexVM()
            {
                PopravniIspitID = PopravniIspitID,
                Rows = db.PopravniIspitStavke.Where(x => x.PopravniIspitID == PopravniIspitID).Select(x => new PopravniIspitStavkeIndexVM.Row()
                {
                    PopravniIspitStavkeID = x.Id,
                    Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                    BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                    PristupIspitu=x.Pristup ? "DA":"NE",
                    Bodovi = x.Bodovi
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Edit(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Where(x => x.Id == PopravniIspitStavkeID)
                                                          .Include(x => x.PopravniIspit)
                                                          .Include(x => x.OdjeljenjeStavka)
                                                          .Include(x => x.OdjeljenjeStavka.Ucenik)
                                                          .FirstOrDefault();

            PopravniIspitStavkeEditVM model = new PopravniIspitStavkeEditVM()
            {
                PopravniIspitStavkeID = PopravniIspitStavkeID,
                Ucenik=stavke.OdjeljenjeStavka.Ucenik.ImePrezime
            };
            return PartialView(model);
        }

        public IActionResult SaveEdit(PopravniIspitStavkeEditVM model)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Find(model.PopravniIspitStavkeID);

            stavke.Bodovi = model.Bodovi;
            db.PopravniIspitStavke.Update(stavke);
            db.SaveChanges();
            return Redirect("/PopravniIspit/Detalji?PopravniIspitID=" + stavke.PopravniIspitID);
        }

        public IActionResult Pristup(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Find(PopravniIspitStavkeID);

            if (stavke.Pristup == true)
            {
                stavke.Pristup = false;
            }
            if (stavke.Pristup == false)
            {
                stavke.Pristup = true;
            }

            db.PopravniIspitStavke.Update(stavke);
            db.SaveChanges();
            return Redirect("/PopravniIspit/Detalji?PopravniIspitID=" + stavke.PopravniIspitID);
        }
    }
}