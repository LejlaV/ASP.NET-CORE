using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
	public class OdjeljenjeStavkeIndexVM
	{
		public int OdjeljenjeID { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int OdjeljenjeStavkeID { get; set; }
			public int BrojUDnevniku { get; set; }
			public string Ucenik { get; set; }
			public int BrojZakljucenihOcjena { get; set; }

		}
	}
}
