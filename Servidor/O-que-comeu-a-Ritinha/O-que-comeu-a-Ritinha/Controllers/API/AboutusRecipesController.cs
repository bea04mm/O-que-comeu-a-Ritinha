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
    public class AboutusRecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AboutusRecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AboutusRecipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AboutusRecipes>>> GetAboutusRecipes()
        {
            return await _context.AboutusRecipes.ToListAsync();
        }

        // GET: api/AboutusRecipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AboutusRecipes>> GetAboutusRecipes(int id)
        {
            var aboutusRecipes = await _context.AboutusRecipes.FindAsync(id);

            if (aboutusRecipes == null)
            {
                return NotFound();
            }

            return aboutusRecipes;
        }

        // PUT: api/AboutusRecipes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAboutusRecipes(int id, AboutusRecipes aboutusRecipes)
        {
            if (id != aboutusRecipes.Id)
            {
                return BadRequest();
            }

            _context.Entry(aboutusRecipes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutusRecipesExists(id))
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

        // POST: api/AboutusRecipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AboutusRecipes>> PostAboutusRecipes(AboutusRecipes aboutusRecipes)
        {
            _context.AboutusRecipes.Add(aboutusRecipes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAboutusRecipes", new { id = aboutusRecipes.Id }, aboutusRecipes);
        }

        // DELETE: api/AboutusRecipes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAboutusRecipes(int id)
        {
            var aboutusRecipes = await _context.AboutusRecipes.FindAsync(id);
            if (aboutusRecipes == null)
            {
                return NotFound();
            }

            _context.AboutusRecipes.Remove(aboutusRecipes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AboutusRecipesExists(int id)
        {
            return _context.AboutusRecipes.Any(e => e.Id == id);
        }
    }
}
