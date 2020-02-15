using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class TakmicenjeUcesnikUpdateVM
	{
		public int TakmicenjeUcesnikID { get; set; }
		public int UcesnikID { get; set; }
		public List<SelectListItem> Ucesnici { get; set; }
		public int Bodovi { get; set; }
	}
}
