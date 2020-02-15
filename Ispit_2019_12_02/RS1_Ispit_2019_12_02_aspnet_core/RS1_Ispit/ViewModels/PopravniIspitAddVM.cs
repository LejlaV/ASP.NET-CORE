using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitAddVM
	{
		public int OdjeljenjeID { get; set; }
		public string SkolskaGodina { get; set; }
		public string Skola { get; set; }
		public string Oznaka { get; set; }
		public int PopravniIspitID { get; set; }
		public int PredmetID { get; set; }
		public List<SelectListItem> Predmeti { get; set; }
		public DateTime Datum { get; set; }
	}
}
