using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitAddVM
	{
		public int PopravniIspitID { get; set; }
		public int SkolskaGodinaID { get; set; }
		public string SkolskaGodina { get; set; }
		public int SkolaID { get; set; }
		public string Skola { get; set; }
		public int PredmetID { get; set; }
		public string Predmet { get; set; }
		public int Nastavnik1ID { get; set; }
		public List<SelectListItem> Nastavnik1 { get; set; }
		public int Nastavnik2ID { get; set; }
		public List<SelectListItem> Nastavnik2 { get; set; }
		public int Nastavnik3ID { get; set; }
		public List<SelectListItem> Nastavnik3 { get; set; }
		public DateTime Datum { get; set; }
	}
}
