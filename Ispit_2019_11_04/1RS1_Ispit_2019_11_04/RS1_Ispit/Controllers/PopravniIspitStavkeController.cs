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
    public class PopravniIspitStavkeController : Controller
    {
        private MojContext db;
        public PopravniIspitStavkeController(MojContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int PopravniIspitID)
        {
            PopravniIspit popravniIspit = db.PopravniIspit.Find(PopravniIspitID);

            PopravniIspitStavkeIndexVM model = new PopravniIspitStavkeIndexVM()
            {
                PopravniIspitID = PopravniIspitID,
                Rows = db.PopravniIspitStavke.Select(x => new PopravniIspitStavkeIndexVM.Row()
                {
                    PopravniIspitStavkeID = x.Id,
                    Ucenik=x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Odjeljenje=x.OdjeljenjeStavka.Odjeljenje.Oznaka,
                    BrojUDnevniku=x.OdjeljenjeStavka.BrojUDnevniku,
                    PristupIspitu=x.Prisutan ? "DA" : "NE",
                    Rezultat=x.Bodovi
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Edit(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Where(x=> x.Id== PopravniIspitStavkeID)
                                                               .Include(x=> x.OdjeljenjeStavka)
                                                               .ThenInclude(x=> x.Ucenik)
                                                               .FirstOrDefault();

            PopravniIspitStavkeEditVM model = new PopravniIspitStavkeEditVM()
            {
                PopravniIspitStavkeID = PopravniIspitStavkeID,
                Ucenik = stavke.OdjeljenjeStavka.Ucenik.ImePrezime
            };

            return PartialView(model);
        }

        public IActionResult Save(PopravniIspitStavkeEditVM model)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Find(model.PopravniIspitStavkeID);

            stavke.Bodovi = model.Bodovi;
            db.PopravniIspitStavke.Update(stavke);
            db.SaveChanges();
            return Redirect("/PopravniIspitStavke/Index?PopravniIspitID=" + stavke.PopravniIspitID);
        }

        public IActionResult PristupIspitu(int PopravniIspitStavkeID)
        {
            PopravniIspitStavke stavke = db.PopravniIspitStavke.Find(PopravniIspitStavkeID);

            if (stavke.Prisutan == true)
            {
                stavke.Prisutan = false;
            }
            else stavke.Prisutan = true;

            db.PopravniIspitStavke.Update(stavke);
            db.SaveChanges();

            return Redirect("/PopravniIspitStavke/Index?PopravniIspitID=" + stavke.PopravniIspitID);
        }
    }
}