using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdrzanaNastavaDetaljiVM
	{
		public int OdrzaniCasID { get; set; }
		public DateTime Datum { get; set; }
		public string PredajePredmet { get; set; }
		public string Sadrzaj { get; set; }

	}
}
