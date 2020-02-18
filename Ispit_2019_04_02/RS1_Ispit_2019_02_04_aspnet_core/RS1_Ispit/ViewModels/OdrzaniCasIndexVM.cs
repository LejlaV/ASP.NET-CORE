using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdrzaniCasIndexVM
	{
		public int NastavnikID { get; set; }
		public string ImePrezime { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int OdrzaniCasID { get; set; }
			public DateTime Datum { get; set; }
			public string Skola { get; set; }
			public string SkolskaGodinaOdjeljenje { get; set; }
			public string Predmet { get; set; }
			public List<string> OdsutniUcenici { get; set; }
		}
	}
}
