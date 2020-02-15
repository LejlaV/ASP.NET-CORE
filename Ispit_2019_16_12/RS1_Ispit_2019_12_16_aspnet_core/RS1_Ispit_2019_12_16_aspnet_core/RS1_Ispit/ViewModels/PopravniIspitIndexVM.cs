﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
	public class PopravniIspitIndexVM
	{
		public int SkolskaGodinaID { get; set; }
		public string SkolskaGodina { get; set; }
		public int SkolaID { get; set; }
		public string Skola { get; set; }
		public int PredmetID { get; set; }
		public string Predmet { get; set; }
		public List<Row> Rows { get; set; }
		public class Row
		{
			public int PopravniIspitID { get; set; }
			public DateTime Datum { get; set; }
			public string Predmet { get; set; }
			public int BrojUcenika { get; set; }
			public int BrojUcenikaPolozeno { get; set; }
		}

	}
}
