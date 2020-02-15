using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class IspitDetaljiIndexVM
	{
		public int IspitID { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int IspitDetaljiID { get; set; }
			public string Student { get; set; }
			public string Pristupio { get; set; }
			public int Ocjena { get; set; }
		}
	}
}
