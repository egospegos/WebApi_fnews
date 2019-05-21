using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp
{
	public class BLL
	{
		private fnewsContext context;

		public BLL(fnewsContext con)
		{
			context = con;
		}

		public int checkCountLike(int id)
		{
			return context.Likes.Where(i => i.NoveltyId == id).Count();
		}
	}
}
