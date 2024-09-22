using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tags>>> GetTags()
        {
            return await _context.Tags
				.OrderBy(t => t.Tag)
				.ToListAsync();
		}

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tags>> GetTags(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var tags = await _context.Tags
				.FirstOrDefaultAsync(m => m.Id == id);

			if (tags == null)
            {
                return NotFound();
            }

            return tags;
        }

		// PUT: api/Tags/PutTags/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("PutTags/{id}")]
        public async Task<IActionResult> PutTags(int id, Tags tags)
        {
            if (id != tags.Id)
            {
                return BadRequest();
            }

			// Normaliza a tag para letras minusculas para comparacao
			var normalizedTag = tags.Tag.Trim().ToLower();

			// Verifica se ja existe uma tag com o mesmo nome (case-insensitive)
			var existingTag = await _context.Tags
				.FirstOrDefaultAsync(t => t.Id != tags.Id && t.Tag.Trim().ToLower() == normalizedTag);

			if (existingTag != null)
			{
				return Conflict(new { message = "Esta tag já existe." });
			}

			_context.Entry(tags).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

		// POST: api/Tags/PostTags
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("PostTags")]
        public async Task<ActionResult<Tags>> PostTags(Tags tags)
        {
			// Normaliza a tag para letras minusculas para comparação
			var normalizedTag = tags.Tag.Trim().ToLower();

			// Verifica se ja existe uma tag com o mesmo nome (case-insensitive)
			var existingTag = await _context.Tags
				.FirstOrDefaultAsync(t => t.Id != tags.Id && t.Tag.Trim().ToLower() == normalizedTag);

			if (existingTag != null)
			{
				return Conflict(new { message = "Esta tag já existe." });
			}

			_context.Tags.Add(tags);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTags", new { id = tags.Id }, tags);
        }

		// DELETE: api/Tags/DeleteIngredients/5
		[HttpDelete("DeleteTags/{id}")]
        public async Task<IActionResult> DeleteTags(int id)
        {
            var tags = await _context.Tags.FindAsync(id);
            if (tags == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tags);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagsExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
