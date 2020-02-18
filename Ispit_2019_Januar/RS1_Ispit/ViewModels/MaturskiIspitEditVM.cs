using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class MaturskiIspitEditVM
	{
		public int MaturskiIspitID { get; set; }
		public DateTime Datum { get; set; }
		public string Predmet { get; set; }
		public string Napomena { get; set; }
	}
}
