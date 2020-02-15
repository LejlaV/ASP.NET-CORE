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
    public class IspitController : Controller
    {
        private MojContext db;
        public IspitController(MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            AngazovanIndexVM model = new AngazovanIndexVM()
            {
                Rows = db.Angazovan.Select(x => new AngazovanIndexVM.Row()
                {
                    AngazovanID = x.Id,
                    Predmet = x.Predmet.Naziv,
                    AkademskaGodina = x.AkademskaGodina.Opis,
                    Nastavnik = x.Nastavnik.Ime + "" + x.Nastavnik.Prezime,
                    BrojOdrzanihCasova = db.OdrzaniCas.Count(y => y.AngazovaniId == x.NastavnikId),
                    BrojStudenataNaPredmetu = db.SlusaPredmet.Count(y => y.AngazovanId == x.Id)
                }).ToList()
            };
            return View(model);
        }
        public IActionResult IspitIndex(int AngazovanID)
        {
            Angazovan angazovan = db.Angazovan.Where(x => x.Id == AngazovanID)
                                               .Include(x=> x.Predmet)
                                               .Include(x=> x.Nastavnik)
                                               .Include(x=> x.AkademskaGodina)
                                               .FirstOrDefault();

            IspitIndexVM model = new IspitIndexVM()
            {
                AngazovanID = AngazovanID,
                Predmet = angazovan.Predmet.Naziv,
                Nastavnik = angazovan.Nastavnik.Ime + " " + angazovan.Nastavnik.Prezime,
                AkademskaGodina = angazovan.AkademskaGodina.Opis,
                Rows = db.Ispit.Where(x => x.AngazovanID == AngazovanID).Select(x => new IspitIndexVM.Row()
                {
                    IspitID = x.Id,
                    Datum = x.Datum,
                    BrojStudenataNisuPolozili = db.IspitDetalji.Where(y => y.IspitID == x.Id && y.Ocjena > 5).Count(),
                    BrojStudenataPrijavljeno = db.IspitDetalji.Where(y => y.IspitID == x.Id).Count(),
                    Zakljuceno=x.Zakljucano ? "DA":"NE"
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Add(int AngazovanID)
        {
            Angazovan angazovan = db.Angazovan.Where(x => x.Id == AngazovanID)
                                               .Include(x => x.Predmet)
                                               .Include(x => x.Nastavnik)
                                               .Include(x => x.AkademskaGodina)
                                               .FirstOrDefault();
            IspitAddVM model = new IspitAddVM()
            {
                AngazovanID = AngazovanID,
                Predmet = angazovan.Predmet.Naziv,
                Nastavnik = angazovan.Nastavnik.Ime + " " + angazovan.Nastavnik.Prezime,
                AkademskaGodina = angazovan.AkademskaGodina.Opis,
            };
            return View(model);
        }

        public IActionResult Save(IspitAddVM model)
        {
            Ispit noviIspit = new Ispit()
            {
                AngazovanID = model.AngazovanID,
                Datum = model.Datum,
                Napomena = model.Napomena
            };
            db.Ispit.Add(noviIspit);
            db.SaveChanges();
            return Redirect("/Ispit/IspitIndex?AngazovanID=" + model.AngazovanID);
        }

        public IActionResult Detalji(int IspitID)
        {
            Ispit ispit = db.Ispit.Where(x => x.Id == IspitID)
                                  .Include(x=> x.Angazovan)
                                  .Include(x=> x.Angazovan.Nastavnik)
                                  .Include(x=> x.Angazovan.Predmet)
                                  .Include(x=> x.Angazovan.AkademskaGodina)
                                  .FirstOrDefault();
            IspitDetaljiVM model = new IspitDetaljiVM()
            {
                IspitID = IspitID,
                Predmet = ispit.Angazovan.Predmet.Naziv,
                Nastavnik = ispit.Angazovan.Nastavnik.Ime + " " + ispit.Angazovan.Nastavnik.Prezime,
                AkademskaGodina = ispit.Angazovan.AkademskaGodina.Opis,
                Datum = ispit.Datum,
                Napomena = ispit.Napomena
            };
            return View(model);
        }

        public IActionResult IspitDetaljiIndex(int IspitID)
        {
            Ispit ispit = db.Ispit.Where(x => x.Id == IspitID)
                                  .Include(x => x.Angazovan)
                                  .Include(x => x.Angazovan.Nastavnik)
                                  .Include(x => x.Angazovan.Predmet)
                                  .Include(x => x.Angazovan.AkademskaGodina)
                                  .FirstOrDefault();

            IspitDetaljiIndexVM model = new IspitDetaljiIndexVM()
            {
                IspitID = IspitID,
                Rows = db.IspitDetalji.Where(x => x.IspitID == IspitID).Select(x => new ViewModels.IspitDetaljiIndexVM.Row()
                {
                    IspitDetaljiID = x.Id,
                    Student = x.Student.Ime + " " + x.Student.Prezime,
                    Ocjena = x.Ocjena,
                    Pristupio = x.Pristupio ? "DA" : "NE"
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult IspitDetaljiAdd(int IspitID)
        {
            Ispit ispit = db.Ispit.Where(x => x.Id == IspitID)
                                  .Include(x => x.Angazovan)
                                  .Include(x => x.Angazovan.Nastavnik)
                                  .Include(x => x.Angazovan.Predmet)
                                  .Include(x => x.Angazovan.AkademskaGodina)
                                  .FirstOrDefault();
            IspitDetaljiAddVM model = new IspitDetaljiAddVM()
            {
                IspitID = IspitID,
                Studenti = db.Student.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.Ime},{x.Prezime}"
                }).ToList()
            };
            return View(model);
        }

        public IActionResult IspitDetaljiSave(IspitDetaljiAddVM model)
        {
            IspitDetalji noviDetalji = new IspitDetalji
            {
                IspitID = model.IspitID,
                StudentID = model.StudentID,
                Ocjena = model.Ocjena,
                Pristupio = true
            };
            db.IspitDetalji.Add(noviDetalji);
            db.SaveChanges();
            return Redirect("/Ispit/Detalji?IspitID=" + model.IspitID);
        }

        public IActionResult Edit(int IspitDetaljiID)
        {
            IspitDetalji ispitDetalji = db.IspitDetalji.Where(x => x.Id == IspitDetaljiID)
                                                       .Include(x=> x.Student)
                                                       .FirstOrDefault();

            IspitDetaljiEditVM model = new IspitDetaljiEditVM()
            {
                IspitDetaljiID = IspitDetaljiID,
                Student = ispitDetalji.Student.Ime + " " + ispitDetalji.Student.Prezime
            };
            return PartialView(model);
        }

        public IActionResult SaveEdit(IspitDetaljiEditVM model)
        {
            IspitDetalji ispitDetalji = db.IspitDetalji.Where(x => x.Id == model.IspitDetaljiID)
                                                       .Include(x => x.Student)
                                                       .Include(x => x.Ispit)
                                                       .FirstOrDefault();
            ispitDetalji.Ocjena = model.Ocjena;
            db.IspitDetalji.Update(ispitDetalji);
            db.SaveChanges();
            return Redirect("/Ispit/Detalji?IspitID=" + ispitDetalji.IspitID);
        }

        public IActionResult Pristupio(int IspitDetaljiID)
        {
            IspitDetalji ispitDetalji = db.IspitDetalji.Where(x => x.Id == IspitDetaljiID)
                                                       .Include(x => x.Student)
                                                       .FirstOrDefault();

            if (ispitDetalji.Pristupio == true)
            {
                ispitDetalji.Pristupio = false;
            }
            else
            {
                ispitDetalji.Pristupio = true;
            }
            db.IspitDetalji.Update(ispitDetalji);
            db.SaveChanges();
            return Redirect("/Ispit/Detalji?IspitID=" + ispitDetalji.IspitID);
        }

        public IActionResult Zakljucaj(int IspitID)
        {
            Ispit ispit = db.Ispit.Where(x => x.Id == IspitID)
                                  .Include(x => x.Angazovan)
                                  .Include(x => x.Angazovan.Nastavnik)
                                  .Include(x => x.Angazovan.Predmet)
                                  .Include(x => x.Angazovan.AkademskaGodina)
                                  .FirstOrDefault();

            ispit.Zakljucano = true;
            db.Ispit.Update(ispit);
            db.SaveChanges();
            return Redirect("Ispit/IspitIndex?IspitID=" + ispit.Id);
        }
    }
}