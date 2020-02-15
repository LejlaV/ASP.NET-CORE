using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class PopravniIspit
	{
		public int Id { get; set; }
		public DateTime Datum { get; set; }

		[ForeignKey(nameof(PredmetID))]
		public virtual Predmet Predmet { get; set; }
		public int PredmetID { get; set; }

		[ForeignKey(nameof(SkolaID))]
		public virtual Skola Skola { get; set; }
		public int SkolaID { get; set; }


		[ForeignKey(nameof(SkolskaGodinaID))]
		public virtual SkolskaGodina SkolskaGodina { get; set; }
		public int SkolskaGodinaID { get; set; }

		[ForeignKey(nameof(Nastavnik1ID))]
		public virtual Nastavnik Nastavnik1 { get; set; }
		public int Nastavnik1ID { get; set; }

		[ForeignKey(nameof(Nastavnik2ID))]
		public virtual Nastavnik Nastavnik2 { get; set; }
		public int Nastavnik2ID { get; set; }

		[ForeignKey(nameof(Nastavnik3ID))]
		public virtual Nastavnik Nastavnik3 { get; set; }
		public int Nastavnik3ID { get; set; }

	}
}
