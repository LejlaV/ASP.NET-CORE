using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.ViewModels;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;

using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class PopravniIspitController : Controller
    {
        private MojContext db;
        public PopravniIspitController (MojContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            PredmetIndexVM model = new PredmetIndexVM()
            {
                Rows = db.Predmet.Select(x => new PredmetIndexVM.Row()
                {
                    PredmetID=x.Id,
                    Razred=x.Razred,
                    NazivPredmeta=x.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult PopravniIspitIndex(int PredmetID)
        {
            Predmet predmet = db.Predmet.Where(x => x.Id == PredmetID).FirstOrDefault();
            PopravniIspitIndexVM model = new PopravniIspitIndexVM()
            {
                PredmetID = PredmetID,
                PredmetRazred = predmet.Naziv + " " + predmet.Razred,
                Rows = db.PopravniIspit.Where(x => x.PredmetID == PredmetID).Select(x => new PopravniIspitIndexVM.Row()
                {
                    PopravniIspitID=x.Id,
                    Skola=x.Skola.Naziv,
                    Skolskagodina=x.SkolskaGodina.Naziv,
                    Datum=x.Datum,
                    BrojUcenika=db.PopravniIspitStavke.Where(y=> y.PopravniIspitID==x.Id).Count(),
                    BrojUcenikaPolozeno=db.PopravniIspitStavke.Where(y=> y.PopravniIspitID==x.Id && y.Bodovi>50).Count()
                }).ToList()
            };

            return View(model);
        }

        public IActionResult Add (int PredmetID)
        {
            Predmet predmet = db.Predmet.Find(PredmetID);

            PopravniIspitAddVM model = new PopravniIspitAddVM()
            {
                PredmetID = PredmetID,
                PredmetNaziv = predmet.Naziv,
                Razred = predmet.Razred,

                Skola = db.Skola.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value=x.Id.ToString(),
                    Text=x.Naziv
                }).ToList(),

                SkolskaGodina = db.SkolskaGodina.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Save (PopravniIspitAddVM model)
        {
            PopravniIspit noviPopravniIspit = new PopravniIspit
            {
                Datum = model.Datum,
                SkolaID = model.SkolaID,
                SkolskaGodinaID = model.SkolskaGodinaID,
                PredmetID=model.PredmetID
            };
            
            db.PopravniIspit.Add(noviPopravniIspit);

            if (model != null)
            {
                var sviPredmeti = db.DodjeljenPredmet.ToList();

                foreach (var uc in sviPredmeti)
                {
                    var uceniciNegativnaZakljucna = db.OdjeljenjeStavka.Where(x => x.Id == uc.OdjeljenjeStavkaId && uc.ZakljucnoKrajGodine == 1).ToList();

                    var uceniciNegZakljucnaPredmet = db.OdjeljenjeStavka.Where(x => x.Id == uc.OdjeljenjeStavkaId && uc.ZakljucnoKrajGodine == 1 && uc.PredmetId == model.PredmetID).ToList();


                    if (uceniciNegZakljucnaPredmet.Any())
                    {
                        PopravniIspitStavke noveStavke = new PopravniIspitStavke
                        {
                            OdjeljenjeStavkaID=uc.OdjeljenjeStavkaId,
                            PopravniIspitID=noviPopravniIspit.Id,
                            Bodovi=null,
                            Prisutan=false
                        };

                        db.PopravniIspitStavke.Add(noveStavke);
                    }

                    if (uceniciNegativnaZakljucna.Count() > 3)
                    {
                        PopravniIspitStavke noveStavke = new PopravniIspitStavke
                        {
                            OdjeljenjeStavkaID = uc.OdjeljenjeStavkaId,
                            PopravniIspitID = noviPopravniIspit.Id,
                            Bodovi = 0,
                            Prisutan = false
                        };

                        db.PopravniIspitStavke.Add(noveStavke);
                    }
                }
            }

            db.SaveChanges();

            return Redirect("/PopravniIspit/PopravniIspitIndex?PredmetID=" + model.PredmetID);
        }

        public IActionResult Detalji(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Where(x=> x.Id==PopravniIspitID)
                                                          .Include(x=> x.Predmet)
                                                          .Include(x=>x.Skola)
                                                          .Include(x=> x.SkolskaGodina)
                                                          .FirstOrDefault();
            PopravniIspitDetaljiVM model = new PopravniIspitDetaljiVM()
            {
                PopravniIspitID=PopravniIspitID,
                Predmet=popravniIspit.Predmet.Naziv,
                Razred=popravniIspit.Predmet.Razred,
                Datum=popravniIspit.Datum,
                Skola=popravniIspit.Skola.Naziv,
                SkolskaGodina=popravniIspit.SkolskaGodina.Naziv
            };
            return View(model);
        }
    }
}
