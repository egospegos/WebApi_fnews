using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreApp.Models
{
	public class Like
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int? NoveltyId { get; set; }
		public int Check { get; set; }
		public virtual User User { get; set; }
		public virtual Novelty Novelty { get; set; }
	}
}
