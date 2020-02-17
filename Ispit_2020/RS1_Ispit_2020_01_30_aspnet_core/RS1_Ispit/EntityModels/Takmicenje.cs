using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class Takmicenje
	{
		public int Id { get; set; }
		public int Razred { get; set; }
		public DateTime Datum { get; set; }
		public bool Zakljucano { get; set; }

		[ForeignKey(nameof(SkolaID))]
		public virtual Skola Skola { get; set; }
		public int SkolaID { get; set; }

		[ForeignKey(nameof(PredmetID))]
		public virtual Predmet Predmet { get; set; }
		public int PredmetID { get; set; }
	}
}
