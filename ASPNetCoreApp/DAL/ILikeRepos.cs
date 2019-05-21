using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp.DAL
{
	public interface ILikeRepos : IDisposable
	{
		IEnumerable<Like> GetAllLikes();
		int GetLikeById(int id);
		int GetLikeById2(Like like);
		Like GetLikeById3(Like like);
		void AddLike(Like like);
		void DeleteLike(Like like);
		void Save();
	}
}
