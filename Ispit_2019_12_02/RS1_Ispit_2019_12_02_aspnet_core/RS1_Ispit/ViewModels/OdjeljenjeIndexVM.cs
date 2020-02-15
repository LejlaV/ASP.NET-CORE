using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdjeljenjeIndexVM
	{
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int OdjeljenjeID { get; set; }
			public string SkolskaGodina { get; set; }
			public string Skola { get; set; }
			public string Oznaka { get; set; }
		}
	}
}
