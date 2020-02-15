using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class OdrzaniCas
	{
		public int Id { get; set; }
		public DateTime Datum { get; set; }
		public string Sadrzaj { get; set; }

		[ForeignKey(nameof(PredajePredmetID))]
		public virtual PredajePredmet PredajePredmet { get; set; }
		public int PredajePredmetID { get; set; }
	}
}
