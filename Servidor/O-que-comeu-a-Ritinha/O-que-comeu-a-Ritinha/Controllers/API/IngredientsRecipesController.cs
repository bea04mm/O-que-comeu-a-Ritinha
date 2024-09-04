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
    public class IngredientsRecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngredientsRecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IngredientsRecipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientsRecipes>>> GetIngredientsRecipes()
        {
            return await _context.IngredientsRecipes.ToListAsync();
        }

        // GET: api/IngredientsRecipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientsRecipes>> GetIngredientsRecipes(int id)
        {
            var ingredientsRecipes = await _context.IngredientsRecipes.FindAsync(id);

            if (ingredientsRecipes == null)
            {
                return NotFound();
            }

            return ingredientsRecipes;
        }

        // PUT: api/IngredientsRecipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredientsRecipes(int id, IngredientsRecipes ingredientsRecipes)
        {
            if (id != ingredientsRecipes.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingredientsRecipes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientsRecipesExists(id))
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

        // POST: api/IngredientsRecipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IngredientsRecipes>> PostIngredientsRecipes(IngredientsRecipes ingredientsRecipes)
        {
            _context.IngredientsRecipes.Add(ingredientsRecipes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIngredientsRecipes", new { id = ingredientsRecipes.Id }, ingredientsRecipes);
        }

        // DELETE: api/IngredientsRecipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredientsRecipes(int id)
        {
            var ingredientsRecipes = await _context.IngredientsRecipes.FindAsync(id);
            if (ingredientsRecipes == null)
            {
                return NotFound();
            }

            _context.IngredientsRecipes.Remove(ingredientsRecipes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientsRecipesExists(int id)
        {
            return _context.IngredientsRecipes.Any(e => e.Id == id);
        }
    }
}
