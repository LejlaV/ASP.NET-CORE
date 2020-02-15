using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniverzitet.Web.Helper;
using Ispit.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Ispit.Web.ViewModels;

namespace Ispit.Web.Controllers
{
    [Autorizacija]
    public class OznaceniDogadajiController : Controller
    {
        private MyContext db;
        public OznaceniDogadajiController (MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            KorisnickiNalog logiraniKorisnik = HttpContext.GetLogiraniKorisnik();

            Student logiraniStudent = db.Student.Where(x => x.KorisnickiNalogId == logiraniKorisnik.Id).FirstOrDefault();

            var sviDogadjaji = db.Dogadjaj.Include(x => x.Nastavnik).ToList();

            var studentDogadjaji = db.OznacenDogadjaj.Where(x => x.StudentID == logiraniStudent.ID).ToList();

            List<DogadjajiIndexVM.Row> neoznaceni = new List<DogadjajiIndexVM.Row>();

            foreach (var svi in sviDogadjaji)
            {
                bool postoji = false;
                foreach (var st in studentDogadjaji)
                {
                    if (st.DogadjajID == svi.ID)
                    {
                        postoji = true;
                    }
                }
                if (postoji == false)
                {
                    DogadjajiIndexVM.Row n = new DogadjajiIndexVM.Row()
                    {
                        DogadjajID = svi.ID,
                        Datum = svi.DatumOdrzavanja,
                        Opis = svi.Opis,
                        Nastavnik = svi.Nastavnik.ImePrezime,
                        BrojObaveza = db.Obaveza.Where(x => x.DogadjajID == svi.ID).Count()
                    };

                    neoznaceni.Add(n);
                }
            }

            DogadjajiIndexVM model = new DogadjajiIndexVM()
            {
                Oznaceni = db.OznacenDogadjaj.Where(x=> x.StudentID==logiraniStudent.ID).Select(x=> new DogadjajiIndexVM.Row()
                {
                    DogadjajID=x.ID,
                    Datum=x.Dogadjaj.DatumOdrzavanja,
                    Nastavnik=x.Dogadjaj.Nastavnik.ImePrezime,
                    Opis=x.Dogadjaj.Opis,
                    Realizovano=db.StanjeObaveze.Where(y=> y.OznacenDogadjajID==x.ID).Sum(y=> y.IzvrsenoProcentualno)
                }).ToList(),
                Neoznaceni=neoznaceni
            };

            return View(model);
        }

        public IActionResult Dodaj(int DogadjajID)
        {
            KorisnickiNalog logiraniKorisnik = HttpContext.GetLogiraniKorisnik();
            Student logiraniStudent = db.Student.Where(x => x.KorisnickiNalogId == logiraniKorisnik.Id).FirstOrDefault();

            OznacenDogadjaj noviOznaceniDogadjaj = new OznacenDogadjaj()
            {
                DogadjajID=DogadjajID,
                StudentID=logiraniStudent.ID,
                DatumDodavanja=DateTime.Now
            };
            db.OznacenDogadjaj.Add(noviOznaceniDogadjaj);

            var obaveze = db.Obaveza.Where(x => x.DogadjajID == noviOznaceniDogadjaj.DogadjajID).ToList();

            foreach (var ob in obaveze)
            {
                StanjeObaveze novoStanjeObaveze = new StanjeObaveze()
                {
                    OznacenDogadjajID=noviOznaceniDogadjaj.ID,
                    ObavezaID=ob.ID,
                    DatumIzvrsenja=DateTime.Now,
                    IsZavrseno = false,
                    IzvrsenoProcentualno = 0,
                    NotifikacijaDanaPrije = 0,
                    NotifikacijeRekurizivno = default
                };
                db.StanjeObaveze.Add(novoStanjeObaveze);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Detalji(int DogadjajID)
        {
            OznacenDogadjaj dogadjaj = db.OznacenDogadjaj.Include(x=> x.Dogadjaj)
                                                         .ThenInclude(x=> x.Nastavnik)
                                                         .Where(x => x.ID == DogadjajID)
                                                         .FirstOrDefault();

            DogadjajDetaljiVM model = new DogadjajDetaljiVM()
            {
                DogadjajID = DogadjajID,
                DatumDodavanja = dogadjaj.DatumDodavanja,
                DatumOdrzavanja = dogadjaj.Dogadjaj.DatumOdrzavanja,
                Opis = dogadjaj.Dogadjaj.Opis,
                Nastavnik = dogadjaj.Dogadjaj.Nastavnik.ImePrezime
            };

            return View(model);
        }

        public IActionResult StanjeObavezaIndex(int DogadjajID)
        {
            OznacenDogadjaj dogadjaj = db.OznacenDogadjaj.Include(x => x.Dogadjaj)
                                                        .ThenInclude(x => x.Nastavnik)
                                                        .Where(x => x.ID == DogadjajID)
                                                        .FirstOrDefault();
            StanjeObavezaIndexVM model = new StanjeObavezaIndexVM()
            {
                Rows = db.StanjeObaveze.Where(x => x.OznacenDogadjajID == DogadjajID).Select(x => new StanjeObavezaIndexVM.Row()
                {
                    StanjeObavezaID=x.Id,
                    Naziv=x.Obaveza.Naziv,
                    ProcenatRealizacije=x.IzvrsenoProcentualno,
                    BrojacNotifikacije=x.NotifikacijaDanaPrije,
                    PonavljajNotifikaciju=x.NotifikacijeRekurizivno ? "DA":"NE"
                }).ToList()
            };
            return PartialView(model);
        }

        public IActionResult Edit(int StanjeObavezeID)
        {
            StanjeObaveze stanjeObaveze = db.StanjeObaveze.Where(x => x.Id == StanjeObavezeID)
                                                          .Include(x => x.Obaveza)
                                                          .FirstOrDefault();

            StanjeObavezeEditVM model = new StanjeObavezeEditVM()
            {
                StanjeObavezeID=StanjeObavezeID,
                Obaveza=stanjeObaveze.Obaveza.Naziv
            };
            return PartialView(model);
        }

        public IActionResult Save(StanjeObavezeEditVM model)
        {
            StanjeObaveze stanjeObaveze = db.StanjeObaveze.Where(x => x.Id == model.StanjeObavezeID)
                                                          .Include(x => x.Obaveza)
                                                          .FirstOrDefault();
            stanjeObaveze.IzvrsenoProcentualno = model.ZavrsenoProcentualno;
            db.StanjeObaveze.Update(stanjeObaveze);
            db.SaveChanges();
            return Redirect("/OznaceniDogadaji/Detalji?DogadjajID=" + stanjeObaveze.OznacenDogadjajID);
        }

        public IActionResult OznaciProcitano(string sadrzaj)
        {
            Obaveza obaveza = db.Obaveza.Where(x => x.Naziv.Equals(sadrzaj)).FirstOrDefault();

            if (obaveza != null)
            {
                var listaStanja = db.StanjeObaveze.Where(x => x.Obaveza.Naziv.Equals(sadrzaj)).ToList();

                foreach (var x in listaStanja)
                {
                    x.IsZavrseno = true;
                    db.StanjeObaveze.Update(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}