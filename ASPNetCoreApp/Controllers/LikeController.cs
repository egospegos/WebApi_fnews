using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNetCoreApp.Models;
using ASPNetCoreApp.DAL;

namespace ASPNetCoreApp.Controllers
{
	[Route("api/[controller]")]
	public class LikeController : Controller
	{

		//private readonly fnewsContext _context;
		private readonly ILikeRepos likeRepos;
		//public LikeController(fnewsContext context)
		//{
		//	_context = context;
		//}
		public LikeController(ILikeRepos likeRepos)
		{
			this.likeRepos = likeRepos;
		}

		[HttpGet]
		public IEnumerable<Like> GetAll()
		{
			//return _context.Likes;
			return likeRepos.GetAllLikes();
		}


		[HttpGet("{id}")]
		public int Get(int id)
		{

			//return _context.Likes.Where(i => i.NoveltyId == id).Count();
			return likeRepos.GetLikeById(id);
		}

		// POST api/<controller>
		public async Task<IActionResult> AddLike([FromBody]Like like)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (likeRepos.GetLikeById2(like) == 0)
				likeRepos.AddLike(like);

			else
			{
				var item = likeRepos.GetLikeById3(like);
				likeRepos.DeleteLike(item);
			}
			if (like.UserId != null)
				//await _context.SaveChangesAsync();
				likeRepos.Save();

			return CreatedAtAction("GetLike", new { id = like.Id }, like);
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
