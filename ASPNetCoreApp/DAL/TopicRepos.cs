using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp.DAL
{
	public class TopicRepos : ITopicRepos, IDisposable
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

		public TopicRepos(fnewsContext con)
		{
			context = con;
		}

		public IEnumerable<Topic> GetAllTopics()
		{
			return context.Topic.Include(p => p.Novelty);
		}

		public Task<Topic> GetTopicById(int id)
		{
			return context.Topic.Include(p => p.Novelty).SingleOrDefaultAsync(m => m.TopicId == id); ;
		}

		public Topic FindTopicById(int id)
		{
			return context.Topic.Find(id);
		}

		

		public void AddTopic(Topic topic)
		{
			context.Topic.Add(topic);
		}

		public void UpdateTopic(Topic topic)
		{
			context.Topic.Update(topic);
		}

		public void DeleteTopic(int id)
		{
			Topic topic = context.Topic.Find(id);
			context.Topic.Remove(topic);
		}

		public void Save()
		{
			context.SaveChangesAsync();
		}
	}
}
