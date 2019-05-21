using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreApp.Models
{
	public partial class Topic
	{
		public Topic()
		{
			Novelty = new HashSet<Novelty>();
		}
		public int TopicId { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public virtual ICollection<Novelty> Novelty { get; set; }
	}
}
