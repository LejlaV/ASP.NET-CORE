using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class SkolaIndexVM
	{
		public int SkolaID { get; set; }
		public List<SelectListItem> Skole { get; set; }
		public int RazredID { get; set; }
		public List<SelectListItem> Razredi { get; set; }

		public SkolaIndexVM()
		{
			Razredi = new List<SelectListItem>();

			for (int i = 1; i < 5; i++)
			{
				Razredi.Add(new SelectListItem
				{
					Value=i.ToString(),
					Text=i.ToString()
				});
			}
		}

	}
}
