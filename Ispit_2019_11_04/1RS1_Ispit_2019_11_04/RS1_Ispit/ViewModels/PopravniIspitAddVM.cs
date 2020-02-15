using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitAddVM
	{
		public int PredmetID { get; set; }
		public string PredmetNaziv { get; set; }
		public int Razred { get; set; }
		public int PopravniIspitID { get; set; }
		public DateTime Datum { get; set; }
		public int SkolaID { get; set; }
		public List<SelectListItem> Skola { get; set; }
		public int SkolskaGodinaID { get; set; }
		public List<SelectListItem> SkolskaGodina { get; set; }

	}
}
