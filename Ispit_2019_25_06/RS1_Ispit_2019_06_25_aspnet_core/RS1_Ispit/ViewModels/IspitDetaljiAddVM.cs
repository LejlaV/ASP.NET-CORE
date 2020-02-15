using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class IspitDetaljiAddVM
	{
		public int IspitDetaljiID { get; set; }
		public int StudentID { get; set; }
		public List<SelectListItem> Studenti { get; set; }
		public int IspitID { get; set; }
		public int Ocjena { get; set; }
	}
}
