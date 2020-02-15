using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.EntityModels
{
	public class IspitDetalji
	{
		public int Id { get; set; }
		public bool Pristupio { get; set; }
		public int Ocjena { get; set; }

		[ForeignKey(nameof(IspitID))]
		public virtual Ispit Ispit { get; set; }
		public int IspitID { get; set; }

		[ForeignKey(nameof(StudentID))]
		public virtual Student Student { get; set; }
		public int StudentID { get; set; }
	}
}
