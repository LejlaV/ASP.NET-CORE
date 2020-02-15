using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class Ispit
	{
		public int Id { get; set; }
		public DateTime Datum { get; set; }
		public string Napomena { get; set; }
		public bool Zakljucano { get; set; }

		[ForeignKey(nameof(AngazovanID))]
		public virtual Angazovan Angazovan { get; set; }
		public int AngazovanID { get; set; }
	}
}
