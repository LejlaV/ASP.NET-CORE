using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.ViewModels;
using RS1_Ispit_asp.net_core.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class MaturskiIspitStavkeController : Controller
    {
        private MojContext db;
        public MaturskiIspitStavkeController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int MaturskiIspitID)
        {
            MaturskiIspit maturskiIspit = db.MaturskiIspit.Where(x => x.Id == MaturskiIspitID)
                                                          .Include(x => x.Nastavnik)
                                                          .FirstOrDefault();
            MaturskiIspitStavkeIndexVM model = new MaturskiIspitStavkeIndexVM()
            {
                MaturskiIspitID = MaturskiIspitID,
                NastavnikID = maturskiIspit.NastavnikID,
                Rows = db.MaturskiIspitStavke.Select(x => new MaturskiIspitStavkeIndexVM.Row()
                {
                    MaturskiIspitStavkeID=x.MaturskiIspitStavkeID,
                    Ucenik = x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    PristupIspitu=x.OdobrenPristup ? "Da" : "Ne",
                    ProsjekOcjena = db.DodjeljenPredmet.Where(y => y.OdjeljenjeStavkaId == x.OdjeljenjeStavkaID).Average(y => y.ZakljucnoKrajGodine),
                    Rezultat = x.BrojBodova ?? 0
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult PristupIspitu(int MaturskiIspitStavkeID)
        {
            MaturskiIspitStavke maturskiIspitStavke = db.MaturskiIspitStavke.Find(MaturskiIspitStavkeID);

            if (maturskiIspitStavke.OdobrenPristup == false)
                maturskiIspitStavke.OdobrenPristup = true;
            else
                maturskiIspitStavke.OdobrenPristup = false;

            db.SaveChanges();
            return Redirect("/MaturskiIspitStavke/Index?MaturskiIspitID=" + maturskiIspitStavke.MaturskiIspitID);
        }

        public IActionResult Edit(int MaturskiIspitStavkeID)
        {
            MaturskiIspitStavke maturskiIspitStavke = db.MaturskiIspitStavke.Include(x => x.OdjeljenjeStavka)
                                                                            .ThenInclude(x => x.Ucenik)
                .Where(x => x.MaturskiIspitStavkeID == MaturskiIspitStavkeID).FirstOrDefault();
            MaturskiIspitStavkeEditVM model = new MaturskiIspitStavkeEditVM()
            {
                MaturskiIspitStavkeID=maturskiIspitStavke.MaturskiIspitStavkeID,
                Ucenik=maturskiIspitStavke.OdjeljenjeStavka.Ucenik.ImePrezime,
                Bodovi=maturskiIspitStavke.BrojBodova ?? 0
            };

            return PartialView(model);
        }

        public IActionResult SaveEdit(MaturskiIspitStavkeEditVM model)
        {
            MaturskiIspitStavke maturskiIspitStavke = db.MaturskiIspitStavke.Find(model.MaturskiIspitStavkeID);
            maturskiIspitStavke.BrojBodova = model.Bodovi;
            db.MaturskiIspitStavke.Update(maturskiIspitStavke);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Edit?MaturskiIspitID=" + maturskiIspitStavke.MaturskiIspitID);
        }
    }
}