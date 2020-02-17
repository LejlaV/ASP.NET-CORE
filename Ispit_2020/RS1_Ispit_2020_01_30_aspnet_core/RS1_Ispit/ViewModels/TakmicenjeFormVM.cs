using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class TakmicenjeFormVM
	{
		public int SkolaID { get; set; }
		public List<SelectListItem> Skola { get; set; }
		public int RazredID { get; set; }
		public List<SelectListItem> Razred { get; set; }

		public TakmicenjeFormVM()
		{
			Razred = new List<SelectListItem>();
			for (int i = 1; i < 5; i++)
			{
				Razred.Add(new SelectListItem()
				{
					Value = i.ToString(),
					Text = i.ToString()
				});
			}
		}
	}
}
