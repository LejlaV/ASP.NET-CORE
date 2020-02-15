using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdrzaniCasAddVM
	{
		public int NastavnikID { get; set; }
		public string ImePrezime { get; set; }
		public int OdrzaniCasID { get; set; }
		public DateTime Datum { get; set; }
		public int PredajePredmetID { get; set; }
		public List<SelectListItem> PredajePredmet { get; set; }
		public string Sadrzaj { get; set; }
	}
}
