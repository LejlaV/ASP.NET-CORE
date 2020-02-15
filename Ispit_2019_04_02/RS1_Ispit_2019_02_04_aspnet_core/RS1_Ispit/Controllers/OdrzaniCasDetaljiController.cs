using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzaniCasDetaljiController : Controller
    {
        private MojContext db;
        public OdrzaniCasDetaljiController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int OdrzaniCasID)
        {
            OdrzaniCas odrzaniCas = db.OdrzaniCas.Find(OdrzaniCasID);
            OdrzaniCasDetaljiIndexVM model = new OdrzaniCasDetaljiIndexVM()
            {
                OdrzaniCasID = OdrzaniCasID,
                Rows = db.OdrzaniCasDetalji.Select(x => new OdrzaniCasDetaljiIndexVM.Row()
                {
                    OdrzaniCasDetaljiID=x.OdrzaniCasDetaljiID,
                    Ucenik=x.OdjeljenjeStavka.Ucenik.ImePrezime,
                    Ocjena=x.Ocjena,
                    Prisutan=x.Pristuan ? "Prisutan" : "Odsutan",
                    OpravdanoOdsutan =x.OpravdanoOdsutan ? "DA" : "NE"
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult PrisutanOdsutan(int OdrzaniCasDetaljiID)
        {
            OdrzaniCasDetalji detalji = db.OdrzaniCasDetalji.Find(OdrzaniCasDetaljiID);

            if (detalji.Pristuan == false)
            {
                detalji.Pristuan = true;
            }
            else detalji.Pristuan = false;

            db.OdrzaniCasDetalji.Update(detalji);
            db.SaveChanges();
            return Redirect("/OdrzanaNastava/Detalji?OdrzaniCasID=" + detalji.OdrzaniCasID);
        }
    }
}