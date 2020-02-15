using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitDetaljiVM
	{
		public int PopravniIspitID { get; set; }
		public string Nastavnik1 { get; set; }
		public string Nastavnik2 { get; set; }
		public string Nastavnik3 { get; set; }
		public DateTime Datum { get; set; }
		public string Skola { get; set; }
		public string SkolskaGodina { get; set; }
		public string Predmet { get; set; }
	}
}
