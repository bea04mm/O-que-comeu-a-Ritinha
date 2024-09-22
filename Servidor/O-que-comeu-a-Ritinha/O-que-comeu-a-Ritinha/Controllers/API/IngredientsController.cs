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
    public class IngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredients>>> GetIngredients()
        {
            return await _context.Ingredients
				.OrderBy(i => i.Ingredient)
				.ToListAsync();
		}

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredients>> GetIngredients(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var ingredients = await _context.Ingredients
				.FirstOrDefaultAsync(m => m.Id == id);

			if (ingredients == null)
            {
                return NotFound();
            }

            return ingredients;
        }

		// PUT: api/Ingredients/PutIngredients/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("PutIngredients/{id}")]
		public async Task<IActionResult> PutIngredients(int id, Ingredients ingredients)
		{
			if (id != ingredients.Id)
			{
				return BadRequest();
			}

			// Normaliza o ingrediente para letras minusculas para comparacao
			var normalizedIngredient = ingredients.Ingredient.Trim().ToLower();

			// Verifica se ja existe um ingrediente com o mesmo nome (case-insensitive)
			var existingIngredient = await _context.Ingredients
				.FirstOrDefaultAsync(i => i.Id != ingredients.Id && i.Ingredient.Trim().ToLower() == normalizedIngredient);

			if (existingIngredient != null)
			{
				return Conflict(new { message = "Este ingrediente já existe." });
			}

			_context.Entry(ingredients).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!IngredientsExists(id))
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

		// POST: api/Ingredients/PostIngredients
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("PostIngredients")]
        public async Task<ActionResult<Ingredients>> PostIngredients(Ingredients ingredients)
        {
			// Normaliza o ingrediente para letras minusculas para comparacao
			var normalizedIngredient = ingredients.Ingredient.Trim().ToLower();

			// Verifica se ja existe um ingrediente com o mesmo nome (case-insensitive)
			var existingIngredient = await _context.Ingredients
				.FirstOrDefaultAsync(i => i.Ingredient.Trim().ToLower() == normalizedIngredient);

			if (existingIngredient != null)
			{
				return Conflict(new { message = "Este ingrediente já existe." });
			}

			_context.Ingredients.Add(ingredients);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredients", new { id = ingredients.Id }, ingredients);
        }

		// DELETE: api/Ingredients/DeleteIngredients/5
		[HttpDelete("DeleteIngredients/{id}")]
        public async Task<IActionResult> DeleteIngredients(int id)
        {
            var ingredients = await _context.Ingredients.FindAsync(id);
            if (ingredients == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredients);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientsExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
