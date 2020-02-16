using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdrzaniCasEditVM
	{
		public int OdrzaniCasID { get; set; }
		public DateTime Datum { get; set; }
		public string OdjeljenjePredmet { get; set; }
		public string Sadrzaj { get; set; }

	}
}
