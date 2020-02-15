﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class OdrzaniCasDetalji
	{
		public int OdrzaniCasDetaljiID { get; set; }
		public bool Pristuan { get; set; }
		public int Ocjena { get; set; }
		public bool OpravdanoOdsutan { get; set; }

		[ForeignKey(nameof(OdrzaniCasID))]
		public virtual OdrzaniCas OdrzaniCas { get; set; }
		public int OdrzaniCasID { get; set; }

		[ForeignKey(nameof(OdjeljenjeStavkaID))]
		public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
		public int OdjeljenjeStavkaID { get; set; }
	}
}