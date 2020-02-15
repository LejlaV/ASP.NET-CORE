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
            OdjeljenjeIndexVM model = new OdjeljenjeIndexVM()
            {
                Rows = db.Odjeljenje.Select(x => new OdjeljenjeIndexVM.Row()
                {
                    OdjeljenjeID = x.Id,
                    Skola = x.Skola.Naziv,
                    SkolskaGodina = x.SkolskaGodina.Naziv,
                    Oznaka = x.Oznaka
                }).ToList()
            };
            return View(model);
        }

        public IActionResult PopravniIspitIndex(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                 .Include(x => x.Skola)
                                                 .Include(x => x.SkolskaGodina).FirstOrDefault();
            PopravniIspitIndexVM model = new PopravniIspitIndexVM() 
            {
                OdjeljenjeID=OdjeljenjeID,
                Skola=odjeljenje.Skola.Naziv,
                SkolskaGodina=odjeljenje.SkolskaGodina.Naziv,
                Rows=db.PopravniIspit.Where(x=> x.OdjeljenjeId==OdjeljenjeID).Select(x=> new PopravniIspitIndexVM.Row()
                {
                    PopravniIspitID=x.Id,
                    Datum=x.Datum,
                    Predmet=x.Predmet.Naziv,
                    BrojUcenika=db.PopravniIspitStavke.Where(y=> y.PopravniIspitID==x.Id).Count(),
                    BrojUcenikaPolozeno=db.PopravniIspitStavke.Where(y=> y.PopravniIspitID==x.Id && y.Bodovi>50).Count()
                }).ToList()
            };
            return View(model);

        }

        public IActionResult Add(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                 .Include(x => x.Skola)
                                                 .Include(x => x.SkolskaGodina).FirstOrDefault();

            PopravniIspitAddVM model = new PopravniIspitAddVM()
            {
                OdjeljenjeID = OdjeljenjeID,
                Skola = odjeljenje.Skola.Naziv,
                SkolskaGodina = odjeljenje.SkolskaGodina.Naziv,
                Oznaka=odjeljenje.Oznaka,
                Predmeti = db.Predmet.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };
            return View(model);
        }
        public IActionResult Save(PopravniIspitAddVM model)
        {
            PopravniIspit noviPopravniIspit = new PopravniIspit()
            {
                OdjeljenjeId = model.OdjeljenjeID,
                PredmetID = model.PredmetID,
                Datum = model.Datum
            };
            db.PopravniIspit.Add(noviPopravniIspit);

            //if (model != null)
            //{
            //    var ucenici = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == model.OdjeljenjeID).ToList();

            //    foreach (var u in ucenici)
            //    {
            //        var NegativneNaPredmetu = db.DodjeljenPredmet
            //            .Where(x => x.OdjeljenjeStavkaId == u.Id && 
            //            x.PredmetId == model.PredmetID && x.ZakljucnoKrajGodine == 1).ToList();

            //        var NegativneNaVisePredmeta = db.DodjeljenPredmet
            //            .Where(x => x.OdjeljenjeStavkaId == u.Id && x.ZakljucnoKrajGodine == 1).ToList();

            //        PopravniIspitStavke stavke;
            //        if (NegativneNaVisePredmeta.Count() >= 3)
            //        {
            //            stavke = new PopravniIspitStavke()
            //            {
            //                PopravniIspitID = noviPopravniIspit.Id,
            //                OdjeljenjeStavkaID = u.Id,
            //                Bodovi = 0,
            //                Pristupio = false
            //            };
            //            db.PopravniIspitStavke.Add(stavke);
            //        }
            //        else if (NegativneNaPredmetu.Any())
            //        {
            //            stavke = new PopravniIspitStavke()
            //            {
            //                PopravniIspitID = noviPopravniIspit.Id,
            //                OdjeljenjeStavkaID = u.Id,
            //                Bodovi = null,
            //                Pristupio = false
            //            };
            //            db.PopravniIspitStavke.Add(stavke);
            //        }
            //    }
            //}

            if (model != null)
            {
                var ucenici = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == model.OdjeljenjeID).ToList();

                foreach (var u in ucenici)
                {
                    PopravniIspitStavke stavke;
                    List<DodjeljenPredmet> predmeti = db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == u.Id && x.ZakljucnoKrajGodine==1).ToList();

                    if (predmeti.Count() >= 3)
                    {
                        stavke = new PopravniIspitStavke()
                        {
                            PopravniIspitID = noviPopravniIspit.Id,
                            OdjeljenjeStavkaID = u.Id,
                            Bodovi = 0,
                            Pristupio = false
                        };
                        db.PopravniIspitStavke.Add(stavke);
                    }
                    else 
                    {
                        stavke = new PopravniIspitStavke()
                        {
                            PopravniIspitID = noviPopravniIspit.Id,
                            OdjeljenjeStavkaID = u.Id,
                            Bodovi = null,
                            Pristupio = false
                        };
                        db.PopravniIspitStavke.Add(stavke);
                    }
                }
            }
            db.SaveChanges();
            return Redirect("/PopravniIspit/PopravniIspitIndex?OdjeljenjeID=" + model.OdjeljenjeID);
        }

        public IActionResult Detalji(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Where(x => x.Id == PopravniIspitID)
                                                          .Include(x => x.Odjeljenje)
                                                          .Include(x => x.Odjeljenje.Skola)
                                                          .Include(x => x.Odjeljenje.SkolskaGodina)
                                                          .Include(x => x.Predmet)
                                                          .FirstOrDefault();

            PopravniIspitDetaljiVM model = new PopravniIspitDetaljiVM()
            {
                PopravniIspitID = PopravniIspitID,
                Oznaka = popravniIspit.Odjeljenje.Oznaka,
                Skola = popravniIspit.Odjeljenje.Skola.Naziv,
                SkolskaGodina = popravniIspit.Odjeljenje.SkolskaGodina.Naziv,
                Datum = popravniIspit.Datum,
                Predmet = popravniIspit.Predmet.Naziv
            };
            return View(model);
        }

        public IActionResult PopravniIspitStavkeIndex(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Where(x => x.Id == PopravniIspitID)
                                                          .Include(x => x.Odjeljenje)
                                                          .Include(x => x.Odjeljenje.Skola)
                                                          .Include(x => x.Odjeljenje.SkolskaGodina)
                                                          .Include(x => x.Predmet)
                                                          .FirstOrDefault();
            PopravniIspitStavkeIndexVM model = new PopravniIspitStavkeIndexVM()
            {
                PopravniIspitID = PopravniIspitID,
                Rows = db.PopravniIspitStavke.Where(x => x.PopravniIspitID == PopravniIspitID)
                                           .Select(x => new PopravniIspitStavkeIndexVM.Row()
                                           {
                                               PopravniIspitStavkeID = x.Id,
                                               Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                                               Odjeljenje = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                                               BrojUDnevniku = x.OdjeljenjeStavka.BrojUDnevniku,
                                               Pristupio = x.Pristupio ? "DA" : "NE",
                                               Rezultat = x.Bodovi
                                           }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Edit(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke popravniIspitStavke = db.PopravniIspitStavke.Where(x => x.Id == PopravniIspitStavkeID)
                                                                            .Include(x=> x.OdjeljenjeStavka)
                                                                            .Include(x=> x.OdjeljenjeStavka.Ucenik)
                                                                            .FirstOrDefault();

            PopravniIspitStavkeEditVM model = new PopravniIspitStavkeEditVM()
            {
                PopravniIspitStavkeID = popravniIspitStavke.Id,
                Ucenik = popravniIspitStavke.OdjeljenjeStavka.Ucenik.ImePrezime
            };
            return PartialView(model);
        }

        public IActionResult SaveEdit(PopravniIspitStavkeEditVM model)
        {
            PopravniIspitStavke popravniIspitStavke = db.PopravniIspitStavke.Where(x => x.Id == model.PopravniIspitStavkeID)
                                                                            .Include(x => x.OdjeljenjeStavka)
                                                                            .Include(x => x.OdjeljenjeStavka.Ucenik)
                                                                            .FirstOrDefault();
            popravniIspitStavke.Bodovi = model.Bodovi;
            db.PopravniIspitStavke.Update(popravniIspitStavke);
            db.SaveChanges();
            return Redirect("/PopravniIspit/Detalji?PopravniIspitID=" + popravniIspitStavke.PopravniIspitID);
        }

        public IActionResult PristupIspitu(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke popravniIspitStavke = db.PopravniIspitStavke.Where(x => x.Id == PopravniIspitStavkeID).FirstOrDefault();

            if (popravniIspitStavke.Pristupio == false)
            {
                popravniIspitStavke.Pristupio = true;
            }
            else popravniIspitStavke.Pristupio = false;

            db.PopravniIspitStavke.Update(popravniIspitStavke);
            db.SaveChanges();
            return Redirect("/PopravniIspit/Detalji?PopravniIspitID=" + popravniIspitStavke.PopravniIspitID);
        }
    }
}