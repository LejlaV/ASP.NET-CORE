using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
	public class StanjeObavezaIndexVM
	{
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int StanjeObavezaID { get; set; }
			public string Naziv { get; set; }
			public float? ProcenatRealizacije { get; set; }
			public int BrojacNotifikacije { get; set; }
			public string PonavljajNotifikaciju { get; set; }
		}
	}
}
