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
            NastavnikIndexVM model = new NastavnikIndexVM()
            {
                Rows = db.Nastavnik.Select(x => new NastavnikIndexVM.Row()
                {
                    NastavnikID = x.Id,
                    ImePrezime = x.Ime + " " + x.Prezime,
                    BrojOdrzanihCasova = db.OdrzaniCas.Where(y => y.PredajePredmet.NastavnikID == x.Id).Count()
                }).ToList()
            };
            return View(model);
        }

        public IActionResult OdrzaniCasIndex(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Find(NastavnikID);

            OdrzaniCasIndexVM model = new OdrzaniCasIndexVM()
            {
                NastavnikID = NastavnikID,
                ImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                Rows = db.OdrzaniCas.Where(x => x.PredajePredmet.NastavnikID == NastavnikID).Select(x => new OdrzaniCasIndexVM.Row()
                {
                    OdrzaniCasID = x.Id,
                    Datum=x.Datum,
                    Skola = x.PredajePredmet.Odjeljenje.Skola.Naziv,
                    SkolskaGodinaOdjeljenje = x.PredajePredmet.Odjeljenje.SkolskaGodina.Naziv + "/" + x.PredajePredmet.Odjeljenje.Oznaka,
                    Predmet = x.PredajePredmet.Predmet.Naziv,
                    OdsutniUcenici = db.OdrzaniCasDetalji.Where(y => y.OdrzaniCasID == x.Id && y.Prisutan == false)
                                                            .Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime)
                                                            .ToList()
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
                NastavnikImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                PredajePredmet = db.PredajePredmet.Where(x => x.NastavnikID == NastavnikID)
                                                          .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                                                          {
                                                              Value = x.Id.ToString(),
                                                              Text = $"{x.Odjeljenje.Skola.Naziv},{x.Odjeljenje.Oznaka},{x.Predmet.Naziv}"
                                                          }).ToList()
            };
            return View(model);
        }

        public IActionResult Save(OdrzaniCasAddVM model)
        {
            OdrzaniCas noviOdrzaniCas = new OdrzaniCas()
            {
                PredajePredmetID = model.PredajePredmetID,
                Datum = model.Datum,
                Sadrzaj = model.Sadrzaj
            };
            db.OdrzaniCas.Add(noviOdrzaniCas);

            var predajePredmet = db.PredajePredmet.Where(x => x.Id == model.PredajePredmetID).FirstOrDefault();

            var ucenici = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == predajePredmet.OdjeljenjeID).ToList();

            foreach (var u in ucenici)
            {
                OdrzaniCasDetalji detalji = new OdrzaniCasDetalji()
                {
                    OdjeljenjeStavkaID = u.Id,
                    OdrzaniCasID = noviOdrzaniCas.Id,
                    Prisutan = false,
                    OpravdanoOdsutan = false,
                    Ocjena = 0
                };
                db.OdrzaniCasDetalji.Add(detalji);
            }
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/OdrzaniCasIndex?NastavnikID=" + noviOdrzaniCas.PredajePredmet.NastavnikID);
        }

        public IActionResult Detalji(int OdrzaniCasID)
        {
            OdrzaniCas cas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasID)
                                          .Include(x=> x.PredajePredmet)
                                          .Include(x=> x.PredajePredmet.Nastavnik)
                                          .Include(x=> x.PredajePredmet.Odjeljenje)
                                          .Include(x=> x.PredajePredmet.Odjeljenje.Skola)
                                          .Include(x=> x.PredajePredmet.Predmet)
                                          .FirstOrDefault();

            DetaljiOdrzaniCasVM model = new DetaljiOdrzaniCasVM()
            {
                OdrzaniCasID = OdrzaniCasID,
                Datum = cas.Datum,
                PredajePredmet = cas.PredajePredmet.Odjeljenje.Skola.Naziv + "/" + cas.PredajePredmet.Odjeljenje.Oznaka + "/" + cas.PredajePredmet.Predmet.Naziv,
                Sadrzaj=cas.Sadrzaj
            };
            return View(model);
        }

        public IActionResult Obrisi (int OdrzaniCasID)
        {
            OdrzaniCas cas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasID)
                                         .Include(x => x.PredajePredmet)
                                         .Include(x => x.PredajePredmet.Nastavnik)
                                         .Include(x => x.PredajePredmet.Odjeljenje)
                                         .Include(x => x.PredajePredmet.Odjeljenje.Skola)
                                         .Include(x => x.PredajePredmet.Predmet)
                                         .FirstOrDefault();

            var detalji = db.OdrzaniCasDetalji.Where(x => x.OdrzaniCasID == OdrzaniCasID).ToList();
            foreach (var d in detalji)
            {
                if (d.OdrzaniCasID == cas.Id)
                {
                    db.OdrzaniCasDetalji.Remove(d);
                }
            }
            db.OdrzaniCas.Remove(cas);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/OdrzaniCasIndex?NastavnikID=" + cas.PredajePredmet.NastavnikID);
        }
        public IActionResult OdrzaniCasDetaljiIndex(int OdrzaniCasID)
        {
            OdrzaniCas cas = db.OdrzaniCas.Find(OdrzaniCasID);

            OdrzaniCasDetaljiIndexVM model = new OdrzaniCasDetaljiIndexVM()
            {
                OdrzaniCasID = OdrzaniCasID,
                Rows = db.OdrzaniCasDetalji.Where(x => x.OdrzaniCasID == OdrzaniCasID).Select(x => new OdrzaniCasDetaljiIndexVM.Row()
                {
                    OdrzaniCasDetaljiID = x.Id,
                    Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Ocjena = x.Ocjena,
                    Prisutan = x.Prisutan ? "Prisutan" : "Odsutan",
                    OpravdanoOdsutan = x.OpravdanoOdsutan ? "DA" : "NE"
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Edit (int OdrzaniCasDetaljiID)
        {
            OdrzaniCasDetalji detalji = db.OdrzaniCasDetalji.Where(x => x.Id == OdrzaniCasDetaljiID)
                                                            .Include(x => x.OdjeljenjeStavka)
                                                            .Include(x => x.OdjeljenjeStavka.Ucenik)
                                                            .FirstOrDefault();

            OdrzaniCasDetaljiEditVM model = new OdrzaniCasDetaljiEditVM()
            {
                OdrzaniCasDetaljiID = OdrzaniCasDetaljiID,
                Ucenik = detalji.OdjeljenjeStavka.Ucenik.ImePrezime,
                Ocjena=detalji.Ocjena,
                Napomena=detalji.Napomena
            };

            if (detalji.Prisutan == true)
            {
                return PartialView("Prisutan", model);
            }
            else
            {
                return PartialView("Odsutan", model);
            }
        }

        public IActionResult SaveEdit(OdrzaniCasDetaljiEditVM model)
        {
            OdrzaniCasDetalji detalji = db.OdrzaniCasDetalji.Where(x => x.Id == model.OdrzaniCasDetaljiID)
                                                           .Include(x => x.OdjeljenjeStavka)
                                                           .Include(x => x.OdjeljenjeStavka.Ucenik)
                                                           .FirstOrDefault();
            if (detalji.Prisutan == true)
            {
                detalji.Ocjena = model.Ocjena;
            }
            else
            {
                detalji.OpravdanoOdsutan = model.OpravdanoOdsutan;
                detalji.Napomena = model.Napomena;
            }
            db.OdrzaniCasDetalji.Update(detalji);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Detalji?OdrzaniCasID=" + detalji.OdrzaniCasID);
        }

        public IActionResult PrisutanOdsutan(int OdrzaniCasDetaljiID)
        {
            OdrzaniCasDetalji detalji = db.OdrzaniCasDetalji.Where(x => x.Id == OdrzaniCasDetaljiID)
                                                            .Include(x => x.OdjeljenjeStavka)
                                                            .Include(x => x.OdjeljenjeStavka.Ucenik)
                                                            .FirstOrDefault();
            if (detalji.Prisutan == true)
            {
                detalji.Prisutan = false;
            }
            else
            {
                detalji.Prisutan = true;
            }
            db.OdrzaniCasDetalji.Update(detalji);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Detalji?OdrzaniCasID=" + detalji.OdrzaniCasID);
        }
    }
}