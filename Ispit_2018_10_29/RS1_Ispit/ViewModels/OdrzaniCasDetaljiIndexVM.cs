using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class OdrzaniCasDetaljiIndexVM
	{
		public int OdrzaniCasID { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int OdrzaniCasDetaljiID { get; set; }
			public string Ucenik { get; set; }
			public int Ocjena { get; set; }
			public string Prisutan { get; set; }
			public string OpravdanoOdsutan { get; set; }
		}
	}
}
