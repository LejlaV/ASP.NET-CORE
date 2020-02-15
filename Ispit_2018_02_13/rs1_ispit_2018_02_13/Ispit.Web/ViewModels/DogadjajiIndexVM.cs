using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
	public class DogadjajiIndexVM
	{
		public List<Row> Oznaceni { get; set; }
		public List<Row> Neoznaceni { get; set; }

		public class Row
		{
			public int DogadjajID { get; set; }
			public DateTime Datum { get; set; }
			public string Nastavnik { get; set; }
			public string Opis { get; set; }
			public int BrojObaveza { get; set; }
			public float? Realizovano { get; set; }
		}
	}
}
