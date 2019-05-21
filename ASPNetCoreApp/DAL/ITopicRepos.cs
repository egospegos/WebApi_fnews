using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp.DAL
{
	public interface ITopicRepos : IDisposable
	{
		IEnumerable<Topic> GetAllTopics();
		Task<Topic> GetTopicById(int id);
		Topic FindTopicById(int id);
	//	string FindCategoryImg(int? id);
		void AddTopic(Topic topic);
		void UpdateTopic(Topic topic);
		void DeleteTopic(int id);
		void Save();
	}
}
