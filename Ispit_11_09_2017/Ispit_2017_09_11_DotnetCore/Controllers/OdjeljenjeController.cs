using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ispit_2017_09_11_DotnetCore.EF;
using Ispit_2017_09_11_DotnetCore.EntityModels;
using Ispit_2017_09_11_DotnetCore.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_09_11_DotnetCore.Controllers
{
    public class OdjeljenjeController : Controller
    {
        private MojContext db;
        public OdjeljenjeController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            OdjeljenjeIndexVM model = new OdjeljenjeIndexVM()
            {
                Rows=db.Odjeljenje.Select(x=> new OdjeljenjeIndexVM.Row()
                {
                    OdjeljenjeID=x.Id,
                    SkolskaGodina=x.SkolskaGodina,
                    Razred=x.Razred,
                    Oznaka=x.Oznaka,
                    Razrednik=x.Nastavnik.ImePrezime,
                    Prebaceni=x.IsPrebacenuViseOdjeljenje ? "DA":"NE",
                    Prosjek=db.DodjeljenPredmet.Where(y=> y.OdjeljenjeStavka.OdjeljenjeId==x.Id).Average(y=> (int?)y.ZakljucnoKrajGodine) ?? 0,
                    NajboljiUcenik = db.DodjeljenPredmet.Where(y=> y.OdjeljenjeStavka.OdjeljenjeId==x.Id)
                                                        .OrderByDescending(z=> z.ZakljucnoKrajGodine)
                                                        .Select(y=> y.OdjeljenjeStavka.Ucenik.ImePrezime)
                                                        .FirstOrDefault()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Add()
        {
            OdjeljenjeAddVM model = new OdjeljenjeAddVM()
            {
                Nastavnici = db.Nastavnik.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.NastavnikID.ToString(),
                    Text = x.ImePrezime
                }).ToList(),

                NizaOdjeljenja = db.Odjeljenje.Where(x => x.IsPrebacenuViseOdjeljenje == false).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.SkolskaGodina},{x.Razred},{x.Oznaka}"
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Save(OdjeljenjeAddVM model)
        {
            Odjeljenje novoOdjeljenje = new Odjeljenje()
            {
                SkolskaGodina=model.SkolskaGodina,
                Razred=model.Razred,
                Oznaka=model.Oznaka,
                NastavnikID=model.NastavnikID,
            };
            db.Odjeljenje.Add(novoOdjeljenje);

            if (model.NizeOdjeljenjeID != null)
            {
                Odjeljenje o = db.Odjeljenje.Where(x => x.Id == model.NizeOdjeljenjeID).FirstOrDefault();
                o.IsPrebacenuViseOdjeljenje = true;
                db.Odjeljenje.Update(o);

                var stavke = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == o.Id).ToList();
                
                foreach (var s in stavke)
                {
                    var opciUspjeh = db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == s.Id).Count(x=> x.ZakljucnoKrajGodine == 1);

                    if (opciUspjeh == 0)
                    {
                        OdjeljenjeStavka noveStavke = new OdjeljenjeStavka()
                        {
                            OdjeljenjeId = novoOdjeljenje.Id,
                            UcenikId = s.UcenikId,
                            BrojUDnevniku = 0
                        };
                        db.OdjeljenjeStavka.Add(noveStavke);
                    }

                    var predmeti = db.Predmet.Where(x => x.Razred == novoOdjeljenje.Razred).ToList();

                    foreach (var p in predmeti)
                    {
                        DodjeljenPredmet noviDodjeljeniPredmet = new DodjeljenPredmet()
                        {
                            OdjeljenjeStavkaId = s.Id,
                            PredmetId = p.Id,
                            ZakljucnoKrajGodine = 0,
                            ZakljucnoPolugodiste = 0
                        };
                        db.DodjeljenPredmet.Add(noviDodjeljeniPredmet);
                    }
                }
            }
            db.SaveChanges();
            return Redirect(nameof(Index));
        }

        public IActionResult Detalji(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                 .Include(x=> x.Nastavnik).FirstOrDefault();

            OdjeljenjeDetaljiVM model = new OdjeljenjeDetaljiVM()
            {
                OdjeljenjeID = OdjeljenjeID,
                SkolskaGodina = odjeljenje.SkolskaGodina,
                Razred = odjeljenje.Razred,
                Oznaka = odjeljenje.Oznaka,
                Razrednik = odjeljenje.Nastavnik.ImePrezime,
                BrojPredmeta = db.Predmet.Where(x => x.Razred == odjeljenje.Razred).Count()
            };
            return View(model);
        }

        public IActionResult Obrisi(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                 .FirstOrDefault();
            var stavke = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == odjeljenje.Id).ToList();
            foreach (var x in stavke)
            {
                if (x.OdjeljenjeId == odjeljenje.Id)
                {
                    db.OdjeljenjeStavka.Remove(x);
                }
            }
            db.Odjeljenje.Remove(odjeljenje);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult OdjeljenjeStavkeIndex(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                 .Include(x => x.Nastavnik).FirstOrDefault();
            OdjeljenjeStavkeIndexVM model = new OdjeljenjeStavkeIndexVM()
            {
                OdjeljenjeID = OdjeljenjeID,
                Rows = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == OdjeljenjeID)
                                          .Select(x => new OdjeljenjeStavkeIndexVM.Row()
                                          {
                                              OdjeljenjeStavkeID = x.Id,
                                              BrojUDnevniku = x.BrojUDnevniku,
                                              Ucenik = x.Ucenik.ImePrezime,
                                              BrojZakljucenihOcjena = db.DodjeljenPredmet.Count(y => y.OdjeljenjeStavkaId == x.Id)
                                          }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult OdjeljenjeStavkeAdd(int OdjeljenjeID)
        {
            Odjeljenje odjeljenje = db.Odjeljenje.Where(x => x.Id == OdjeljenjeID)
                                                  .Include(x => x.Nastavnik).FirstOrDefault();
            OdjeljenjeStavkeAddVM model = new OdjeljenjeStavkeAddVM()
            {
                OdjeljenjeID = OdjeljenjeID,
                Ucenik = db.Ucenik.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.ImePrezime
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult OdjeljenjeStavkeSave(OdjeljenjeStavkeAddVM model)
        {
            OdjeljenjeStavka novaStavka = new OdjeljenjeStavka()
            {
                OdjeljenjeId = model.OdjeljenjeID,
                UcenikId = model.UcenikID,
                BrojUDnevniku = model.BrojUDnevniku
            };
            db.OdjeljenjeStavka.Add(novaStavka);
            db.SaveChanges();
            return Redirect("/Odjeljenje/Detalji?OdjeljenjeID=" + model.OdjeljenjeID);
        }

        public IActionResult OdjeljenjeStavkeEdit(int OdjeljenjeStavkeID)
        {
            OdjeljenjeStavka stavka = db.OdjeljenjeStavka.Where(x => x.Id == OdjeljenjeStavkeID)
                                                         .Include(x=> x.Ucenik).FirstOrDefault();

            OdjeljenjeStavkeEditVM model = new OdjeljenjeStavkeEditVM()
            {
                OdjeljenjeStavkeID=OdjeljenjeStavkeID,
                Ucenik=stavka.Ucenik.ImePrezime
            };
            return PartialView(model);
        }

        public IActionResult OdjeljenjeStavkeSaveEdit(OdjeljenjeStavkeEditVM model)
        {
            OdjeljenjeStavka stavka = db.OdjeljenjeStavka.Where(x => x.Id == model.OdjeljenjeStavkeID)
                                                         .Include(x => x.Ucenik).FirstOrDefault();
            stavka.BrojUDnevniku = model.BrojUDnevniku;
            db.OdjeljenjeStavka.Update(stavka);
            db.SaveChanges();
            return Redirect("/Odjeljenje/Detalji?OdjeljenjeID=" + stavka.OdjeljenjeId);
        }

        public IActionResult OdjeljenjeStavkeObrisi(int OdjeljenjeStavkeID)
        {
            OdjeljenjeStavka stavka = db.OdjeljenjeStavka.Where(x => x.Id == OdjeljenjeStavkeID)
                                                         .Include(x => x.Ucenik).FirstOrDefault();
            db.OdjeljenjeStavka.Remove(stavka);
            db.SaveChanges();
            return Redirect("/Odjeljenje/Detalji?OdjeljenjeID=" + stavka.OdjeljenjeId);
        }
    }
}