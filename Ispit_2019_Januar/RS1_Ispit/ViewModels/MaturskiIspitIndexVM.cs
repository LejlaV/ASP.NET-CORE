using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class MaturskiIspitIndexVM
	{
		public int NastavnikID { get; set; }
		public string NastavnikImePrezime { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int MaturskiIspitID { get; set; }
			public DateTime Datum { get; set; }
			public string Skola { get; set; }
			public string Predmet { get; set; }
			public List<string> Ucenici { get; set; }
		}
	}
}
