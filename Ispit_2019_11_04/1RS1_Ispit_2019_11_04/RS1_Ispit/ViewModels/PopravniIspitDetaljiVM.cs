using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitDetaljiVM
	{
		public int PopravniIspitID { get; set; }
		public string Predmet { get; set; }
		public int Razred { get; set; }
		public DateTime Datum { get; set; }
		public string Skola { get; set; }
		public string SkolskaGodina { get; set; }
	}
}
