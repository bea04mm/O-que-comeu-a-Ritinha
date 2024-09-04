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
    public class RecipesTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipesTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RecipesTags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipesTags>>> GetRecipesTags()
        {
            return await _context.RecipesTags.ToListAsync();
        }

        // GET: api/RecipesTags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipesTags>> GetRecipesTags(int id)
        {
            var recipesTags = await _context.RecipesTags.FindAsync(id);

            if (recipesTags == null)
            {
                return NotFound();
            }

            return recipesTags;
        }

        // PUT: api/RecipesTags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipesTags(int id, RecipesTags recipesTags)
        {
            if (id != recipesTags.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipesTags).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipesTagsExists(id))
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

        // POST: api/RecipesTags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipesTags>> PostRecipesTags(RecipesTags recipesTags)
        {
            _context.RecipesTags.Add(recipesTags);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipesTags", new { id = recipesTags.Id }, recipesTags);
        }

        // DELETE: api/RecipesTags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipesTags(int id)
        {
            var recipesTags = await _context.RecipesTags.FindAsync(id);
            if (recipesTags == null)
            {
                return NotFound();
            }

            _context.RecipesTags.Remove(recipesTags);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipesTagsExists(int id)
        {
            return _context.RecipesTags.Any(e => e.Id == id);
        }
    }
}
