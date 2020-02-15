using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit_2017_09_11_DotnetCore.ViewModels
{
	public class OdjeljenjeIndexVM
	{
		public List<Row> Rows { get; set; }

		public class Row
		{
			public int OdjeljenjeID { get; set; }
			public string SkolskaGodina { get; set; }
			public int Razred { get; set; }
			public string Oznaka { get; set; }
			public string Razrednik { get; set; }
			public string Prebaceni { get; set; }
			public double Prosjek { get; set; }
			public string NajboljiUcenik { get; set; }
		}

	}
}
