using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp.DAL
{
	public class LikeRepos : ILikeRepos, IDisposable
	{
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
						context.Dispose();
					}
					catch (Exception ex)
					{
						Log.Write(ex);
					}
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private fnewsContext context;

		public LikeRepos(fnewsContext con)
		{
			context = con;
		}

		public IEnumerable<Like> GetAllLikes()
		{
			return context.Likes;
		}

		public int GetLikeById(int id)
		{
			return context.Likes.Where(i => i.NoveltyId == id).Count();

		}

		public int GetLikeById2(Like like)
		{
			return context.Likes.Where(i => i.NoveltyId == like.NoveltyId).Where(j => j.UserId == like.UserId).Count();

		}

		public Like GetLikeById3(Like like)
		{
			return context.Likes.Find(context.Likes.Where(i => i.NoveltyId == like.NoveltyId).Where(j => j.UserId == like.UserId).FirstOrDefault().Id);

		}

		public void AddLike(Like like)
		{
			context.Likes.Add(like);
		}

		public void DeleteLike(Like like)
		{
			//Like like = context.Likes.Find(id);
			context.Likes.Remove(like);
		}

		public void Save()
		{
			context.SaveChangesAsync();
		}
	}
}
