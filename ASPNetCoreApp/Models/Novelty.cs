using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreApp.Models
{
	public class Novelty
	{
		public Novelty()
		{
			Likes = new HashSet<Like>();
		}
		public int NoveltyId { get; set; }
		public int TopicId { get; set; }
		public string UserId { get; set; }
		public string Content { get; set; }
		public string Title { get; set; }
		public virtual Topic Topic { get; set; }
		public virtual ICollection<Like> Likes { get; set; }
		public virtual User User { get; set; }
	}
}
