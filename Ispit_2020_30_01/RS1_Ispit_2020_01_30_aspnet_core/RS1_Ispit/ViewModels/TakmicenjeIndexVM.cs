using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class TakmicenjeIndexVM
	{
		public int SkolaID { get; set; }
		public string Skola { get; set; }
		public int? Razred { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int TakmicenjeID { get; set; }
			public string Predmet { get; set; }
			public int Razred { get; set; }
			public DateTime Datum { get; set; }
			public int BrojUcesnikaNisuPristupili { get; set; }
			public string NajboljaSkola { get; set; }
			public string NajboljeOdjeljenje { get; set; }
			public string NajboljiUcesnik { get; set; }
		}
	}
}
