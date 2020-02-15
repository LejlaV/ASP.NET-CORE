using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
	public class DogadjajDetaljiVM
	{
		public int DogadjajID { get; set; }
		public DateTime DatumDodavanja { get; set; }
		public DateTime DatumOdrzavanja { get; set; }
		public string Opis { get; set; }
		public string Nastavnik { get; set; }
	}
}
