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
                    Skola = x.Skola.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult OdrzaniCasIndex(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Where(x => x.Id == NastavnikID)
                                              .Include(x => x.Skola)
                                              .FirstOrDefault();
            OdrzaniCasIndexVM model = new OdrzaniCasIndexVM()
            {
                NastavnikID = NastavnikID,
                ImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                Skola = nastavnik.Skola.Naziv,
                Rows = db.OdrzaniCas.Where(x => x.PredajePredmet.NastavnikID == NastavnikID)
                                    .Select(x => new OdrzaniCasIndexVM.Row()
                                    {
                                        OdrzaniCasID = x.Id,
                                        Datum = x.Datum,
                                        SkolskaGodinaOdjeljenje = x.PredajePredmet.Odjeljenje.SkolskaGodina.Naziv + "/" + x.PredajePredmet.Odjeljenje.Oznaka,
                                        Predmet=x.PredajePredmet.Predmet.Naziv,
                                        OdsutniUcenici = db.OdrzaniCasDetalji.Where(y => y.OdrzaniCasID == x.Id && y.Prisutan == false).Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()
                                    }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int NastavnikID)
        {
            Nastavnik nastavnik = db.Nastavnik.Where(x => x.Id == NastavnikID)
                                              .FirstOrDefault();
            OdrzaniCasAddVM model = new OdrzaniCasAddVM()
            {
                NastavnikID = NastavnikID,
                NastavnikImePrezime = nastavnik.Ime + " " + nastavnik.Prezime,
                OdjeljenjePredmeti = db.PredajePredmet.Where(x => x.NastavnikID == NastavnikID).Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.Odjeljenje.Oznaka},{x.Predmet.Naziv}"
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Save(OdrzaniCasAddVM model)
        {
            OdrzaniCas noviOdrzaniCas = new OdrzaniCas()
            {
                PredajePredmetID = model.OdjeljenjePredmetID,
                Datum = model.Datum
            };
            db.OdrzaniCas.Add(noviOdrzaniCas);

            var OdjeljenjeID = db.PredajePredmet.Where(x => x.Id == model.OdjeljenjePredmetID).Select(x => x.OdjeljenjeID).SingleOrDefault();

            var ucenici = db.OdjeljenjeStavka.Where(x => x.OdjeljenjeId == OdjeljenjeID).ToList();

            foreach (var u in ucenici)
            {
                OdrzaniCasDetalji noviDetalji = new OdrzaniCasDetalji()
                {
                    OdjeljenjeStavkaID = u.Id,
                    OdrzaniCasID = noviOdrzaniCas.Id
                };
                db.OdrzaniCasDetalji.Add(noviDetalji);
            }

            db.SaveChanges();
            return Redirect("/OdrzanaNastava/OdrzaniCasIndex?NastavnikID=" + model.NastavnikID);
        }

        public IActionResult Obrisi (int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x=> x.Id==OdrzaniCasID)
                                                 .Include(x=> x.PredajePredmet)
                                                 .Include(x=> x.PredajePredmet.Nastavnik)
                                                 .Include(x=> x.PredajePredmet.Odjeljenje)
                                                 .Include(x=> x.PredajePredmet.Predmet)
                                                 .FirstOrDefault();

            db.OdrzaniCas.Remove(odrzaniCas);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/OdrzaniCasIndex?NastavnikID=" + odrzaniCas.PredajePredmet.NastavnikID);
        }

        public IActionResult Edit(int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasID)
                                                 .Include(x => x.PredajePredmet)
                                                 .Include(x => x.PredajePredmet.Nastavnik)
                                                 .Include(x => x.PredajePredmet.Odjeljenje)
                                                 .Include(x => x.PredajePredmet.Predmet)
                                                 .FirstOrDefault();

            OdrzaniCasEditVM model = new OdrzaniCasEditVM()
            {
                OdrzaniCasID = OdrzaniCasID,
                Datum = odrzaniCas.Datum,
                OdjeljenjePredmet = odrzaniCas.PredajePredmet.Odjeljenje.Oznaka + " / " + odrzaniCas.PredajePredmet.Predmet.Naziv
            };
            return View(model);
        }

        public IActionResult SaveEdit(OdrzaniCasEditVM model)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x => x.Id == model.OdrzaniCasID)
                                                 .Include(x => x.PredajePredmet)
                                                 .Include(x => x.PredajePredmet.Nastavnik)
                                                 .Include(x => x.PredajePredmet.Odjeljenje)
                                                 .Include(x => x.PredajePredmet.Predmet)
                                                 .FirstOrDefault();

            odrzaniCas.Sadrzaj = model.Sadrzaj;
            db.OdrzaniCas.Update(odrzaniCas);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/OdrzaniCasIndex?NastavnikID=" + odrzaniCas.PredajePredmet.NastavnikID);
        }

        public IActionResult OdrzaniCasDetaljiIndex(int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Where(x => x.Id == OdrzaniCasID)
                                                .Include(x => x.PredajePredmet)
                                                .Include(x => x.PredajePredmet.Nastavnik)
                                                .Include(x => x.PredajePredmet.Odjeljenje)
                                                .Include(x => x.PredajePredmet.Predmet)
                                                .FirstOrDefault();

            OdrzaniCasDetaljiIndexVM model = new OdrzaniCasDetaljiIndexVM()
            {
                OdrzaniCasID = OdrzaniCasID,
                Rows = db.OdrzaniCasDetalji.Where(x => x.OdrzaniCasID == OdrzaniCasID).Select(x => new OdrzaniCasDetaljiIndexVM.Row()
                {
                    OdrzaniCasDetaljiID = x.Id,
                    Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Ocjena = x.Ocjena,
                    Prisutan = x.Prisutan ? "Prisutan" : "Odsutan",
                    OpravdanoOdsutan = x.Opravdano ? "DA" : "NE"
                }).ToList()
            };
            return View(model);
        }

        public IActionResult OdrzaniCasDetaljiEdit(int OdrzaniCasDetaljiID)
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

        public IActionResult OdrzaniCasDetaljiSave(OdrzaniCasDetaljiEditVM model)
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
                detalji.Napomena = model.Napomena;
                detalji.Opravdano = model.Opravdano;
            }
            db.OdrzaniCasDetalji.Update(detalji);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Edit?OdrzaniCasID=" + detalji.OdrzaniCasID);
        }

        public IActionResult Prisutan(int OdrzaniCasDetaljiID)
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
            return Redirect("/OdrzanaNastava/Edit?OdrzaniCasID=" + detalji.OdrzaniCasID);
        }
    }
}