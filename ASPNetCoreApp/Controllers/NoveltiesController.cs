using ASPNetCoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ASPNetCoreApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NoveltiesController : ControllerBase
	{
		private readonly fnewsContext _context;
		public NoveltiesController(fnewsContext context)
		{
			_context = context;
			if (_context.Novelty.Count() == 0)
			{
				_context.Novelty.Add(new Novelty
				{
					Title = "Заголовок новости"
				});
				_context.SaveChanges();
			}
		}

		[HttpGet]
		public IEnumerable<Novelty> GetAll()
		{
			return _context.Novelty;
		}

		[HttpGet]
		[Route("{TopicId}/novelty")]
		public async Task<IActionResult> GetNovelty([FromRoute] int TopicId, int NoveltyId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var novelty = await _context.Novelty.Include(p => p.Topic).SingleOrDefaultAsync(m => m.NoveltyId == NoveltyId);
			if (novelty == null)
			{
				return NotFound();
			}

			var topic = await _context.Topic.Include(p => p.Novelty).SingleOrDefaultAsync(m => m.TopicId == TopicId);
			if (topic == null)
			{
				return NotFound();
			}

			return Ok(novelty);
		}

		[HttpGet]
		[Route("{TopicId}/novelties")]
		public async Task<IActionResult> GetTopicNovelties([FromRoute] int TopicId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var topic = await _context.Topic.Include(p => p.Novelty).SingleOrDefaultAsync(m => m.TopicId == TopicId);
			if (topic == null)
			{
				return NotFound();
			}

			var novelty = await _context.Novelty.Where(m => m.TopicId == TopicId).ToListAsync();
			if (novelty == null)
			{
				return NotFound();
			}
			return Ok(novelty);
		}

		[HttpGet]
		[Route("{TopicId}/novelties/{NoveltyId}")]
		public async Task<IActionResult> GetTopicOneNovelty([FromRoute] int TopicId, int NoveltyId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var topic = await _context.Topic.Include(p => p.Novelty).SingleOrDefaultAsync(m => m.TopicId == TopicId);
			if (topic == null)
			{
				return NotFound();
			}

			var novelty = await _context.Novelty.SingleOrDefaultAsync(m => m.NoveltyId == NoveltyId);
			if (novelty == null)
			{
				return NotFound();
			}
			return Ok(novelty);
		}

		[HttpPost]
		[Route("{TopicId}/novelty")]
		public async Task<IActionResult> CreateNovelty([FromBody] Novelty novelty, int TopicId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var item = _context.Topic.Find(TopicId);
			if (item == null)
			{
				return NotFound();
			}

			novelty.TopicId = item.TopicId;
			_context.Novelty.Add(novelty);
			await _context.SaveChangesAsync();
			//		return CreatedAtAction("GetNovelty", new { id = novelty.NoveltyId }, novelty);
			return NoContent();
		}


		[Authorize(Roles = "admin")]
		[HttpPut]
		[Route("{TopicId}/novelties/{NoveltyId}")]
		public async Task<IActionResult> UpdateNovelty([FromRoute] int TopicId, int NoveltyId, [FromBody] Novelty novelty)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Topic.Find(TopicId);
			if (item == null)
			{
				return NotFound();
			}

			var Nitem = _context.Novelty.Find(NoveltyId);
			if (Nitem == null)
			{
				return NotFound();
			}

			if (novelty.Title != null) Nitem.Title = novelty.Title;
			if (novelty.Content != null) Nitem.Content = novelty.Content;
			//	if (novelty.TopicId != null) Nitem.TopicId = novelty.TopicId;
			_context.Novelty.Update(Nitem);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[Authorize(Roles = "admin")]
		[HttpDelete]
		[Route("{TopicId}/novelties/{NoveltyId}")]
		public async Task<IActionResult> Delete([FromRoute] int TopicId, int NoveltyId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Novelty.Find(NoveltyId);
			if (item == null)
			{
				return NotFound();
			}
			_context.Novelty.Remove(item);
			await _context.SaveChangesAsync();
			return NoContent();
		}

	}

}
