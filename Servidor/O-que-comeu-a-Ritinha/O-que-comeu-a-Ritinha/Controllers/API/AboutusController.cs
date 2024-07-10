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
    public class AboutusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AboutusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Aboutus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aboutus>>> GetAboutus()
        {
            return await _context.Aboutus.ToListAsync();
        }

        // GET: api/Aboutus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aboutus>> GetAboutus(int id)
        {
            var aboutus = await _context.Aboutus.FindAsync(id);

            if (aboutus == null)
            {
                return NotFound();
            }

            return aboutus;
        }

        // PUT: api/Aboutus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAboutus(int id, Aboutus aboutus)
        {
            if (id != aboutus.Id)
            {
                return BadRequest();
            }

            _context.Entry(aboutus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutusExists(id))
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

        // POST: api/Aboutus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Aboutus>> PostAboutus(Aboutus aboutus)
        {
            _context.Aboutus.Add(aboutus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAboutus", new { id = aboutus.Id }, aboutus);
        }

        // DELETE: api/Aboutus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAboutus(int id)
        {
            var aboutus = await _context.Aboutus.FindAsync(id);
            if (aboutus == null)
            {
                return NotFound();
            }

            _context.Aboutus.Remove(aboutus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AboutusExists(int id)
        {
            return _context.Aboutus.Any(e => e.Id == id);
        }
    }
}
