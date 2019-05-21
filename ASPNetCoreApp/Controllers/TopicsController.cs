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
	public class TopicsController : ControllerBase
	{
		private readonly fnewsContext _context;
		public TopicsController(fnewsContext context)
		{
			_context = context;
			if (_context.Topic.Count() == 0)
			{
				_context.Topic.Add(new Topic
				{
					Url = "http:\\topics.net"
				});
				_context.SaveChanges();
			}
		}

		[HttpGet]
		public IEnumerable<Topic> GetAll()
		{
			return _context.Topic.Include(p => p.Novelty);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetTopic([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var topic = await _context.Topic.Include(p=>p.Novelty).SingleOrDefaultAsync(m =>m.TopicId == id);
			if (topic == null)
			{
				return NotFound();
			}
			return Ok(topic);
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
			if(novelty==null)
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

		[Authorize(Roles = "admin")]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			_context.Topic.Add(topic);
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetTopic", new { id = topic.TopicId },topic);
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
		[Route("{id}")]
		public async Task<IActionResult> UpdateTopic([FromRoute] int id,[FromBody] Topic topic)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Topic.Find(id);
			if (item == null)
			{
				return NotFound();
			}
			if (topic.Url != null) item.Url = topic.Url;
			if (topic.Name != null) item.Name = topic.Name;
			if (topic.Novelty != null) item.Novelty = topic.Novelty;
			_context.Topic.Update(item);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[Authorize(Roles = "admin")]
		[HttpPut]
		[Route("{TopicId}/novelties/{NoveltyId}")]
		public async Task<IActionResult> UpdateNovelty([FromRoute] int TopicId, int NoveltyId,[FromBody] Novelty novelty)
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
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Topic.Find(id);
			if (item == null)
			{
				return NotFound();
			}
			_context.Topic.Remove(item);
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
