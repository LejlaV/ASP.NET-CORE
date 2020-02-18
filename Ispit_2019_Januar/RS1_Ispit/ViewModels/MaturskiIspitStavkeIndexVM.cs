using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class MaturskiIspitStavkeIndexVM
	{
		public int MaturskiIspitID { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int MaturskiIspitStavkeID { get; set; }
			public string Ucenik { get; set; }
			public double ProsjekOcjena { get; set; }
			public string PristupIspitu { get; set; }
			public int? Bodovi { get; set; }
		}
	}
}
