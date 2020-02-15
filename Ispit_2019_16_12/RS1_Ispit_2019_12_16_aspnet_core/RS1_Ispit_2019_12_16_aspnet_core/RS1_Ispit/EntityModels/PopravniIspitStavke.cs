using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class PopravniIspitStavke
	{
		public int Id { get; set; }
		public bool Pristup { get; set; }
		public int? Bodovi { get; set; }

		[ForeignKey(nameof(PopravniIspitID))]
		public virtual PopravniIspit PopravniIspit { get; set; }
		public int PopravniIspitID { get; set; }

		[ForeignKey(nameof(OdjeljenjeStavkaID))]
		public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
		public int OdjeljenjeStavkaID { get; set; }
	}
}
