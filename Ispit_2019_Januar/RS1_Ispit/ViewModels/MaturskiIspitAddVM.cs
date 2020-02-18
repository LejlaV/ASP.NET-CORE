using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class MaturskiIspitAddVM
	{
		public int MaturskiIspitID { get; set; }
		public int NastavnikID { get; set; }
		public string NastavnikImePrezime { get; set; }
		public string SkolskaGodina { get; set; }
		public int SkolaID { get; set; }
		public List<SelectListItem> Skola { get; set; }
		public DateTime Datum { get; set; }
		public int PredmetID { get; set; }
		public List<SelectListItem> Predmet { get; set; }

	}
}
