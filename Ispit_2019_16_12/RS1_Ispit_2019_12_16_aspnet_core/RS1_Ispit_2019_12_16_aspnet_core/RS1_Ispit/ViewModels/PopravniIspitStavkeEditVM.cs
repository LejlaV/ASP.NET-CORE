﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitStavkeEditVM
	{
		public int PopravniIspitStavkeID { get; set; }
		public string Ucenik { get; set; }
		public int Bodovi { get; set; }
	}
}