using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class IspitIndexVM
	{
		public int AngazovanID { get; set; }
		public string Predmet { get; set; }
		public string AkademskaGodina { get; set; }
		public string Nastavnik { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int IspitID { get; set; }
			public DateTime Datum { get; set; }
			public int BrojStudenataNisuPolozili { get; set; }
			public int BrojStudenataPrijavljeno { get; set; }
			public string Zakljuceno { get; set; }
		}
	}
}
