using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class TakmicenjeUcesnik
	{
		public int Id { get; set; }
		public bool Pristupio { get; set; }
		public int Bodovi { get; set; }

		[ForeignKey(nameof(TakmicenjeID))]
		public virtual Takmicenje Takmicenje { get; set; }
		public int TakmicenjeID { get; set; }

		[ForeignKey(nameof(OdjeljenjeStavkaID))]
		public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }
		public int OdjeljenjeStavkaID { get; set; }
	}
}
