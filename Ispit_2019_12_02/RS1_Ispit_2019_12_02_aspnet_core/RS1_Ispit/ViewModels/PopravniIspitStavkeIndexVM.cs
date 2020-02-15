using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitStavkeIndexVM
	{
		public int PopravniIspitID { get; set; }

		public List<Row> Rows { get; set; }
		public class Row
		{
			public int PopravniIspitStavkeID { get; set; }
			public string Ucenik { get; set; }
			public string Odjeljenje { get; set; }
			public int BrojUDnevniku { get; set; }
			public string Pristupio { get; set; }
			public int? Rezultat { get; set; }
		}

	}
}
