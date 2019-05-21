using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ASPNetCoreApp.Models
{
	public class User : IdentityUser
	{
		public User()
		{
			Novelties = new HashSet<Novelty>();
			Likes = new HashSet<Like>();
		}
		public virtual ICollection<Novelty> Novelties { get; set; }
		public virtual ICollection<Like> Likes { get; set; }
	}
}
