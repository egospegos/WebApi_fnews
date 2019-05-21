using ASPNetCoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		[HttpGet("{id}")]
		[Route("api/topics/{TopicId}/novelties/{NoveltyId}")]
		public async Task<IActionResult> GetNovelty([FromRoute] int id, int TopicId, int NoveltyId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var novelty = await _context.Novelty.SingleOrDefaultAsync(m => m.NoveltyId == id);
			if (novelty == null)
			{
				return NotFound();
			}
			return Ok(novelty);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] Novelty novelty)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			_context.Novelty.Add(novelty);
			await _context.SaveChangesAsync();
			return CreatedAtAction("GetNovelty", new { id = novelty.NoveltyId },
			novelty);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Novelty novelty)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Novelty.Find(id);
			if (item == null)
			{
				return NotFound();
			}
			item.Title = novelty.Title;
			_context.Novelty.Update(item);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var item = _context.Novelty.Find(id);
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
