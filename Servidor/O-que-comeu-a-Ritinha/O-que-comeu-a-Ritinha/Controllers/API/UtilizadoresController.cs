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
    public class UtilizadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Utilizadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizadores>>> GetUtilizadores()
        {
            return await _context.Utilizadores.ToListAsync();
        }

        // GET: api/Utilizadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizadores>> GetUtilizadores(int id)
        {
            var utilizadores = await _context.Utilizadores.FindAsync(id);

            if (utilizadores == null)
            {
                return NotFound();
            }

            return utilizadores;
        }

        // PUT: api/Utilizadores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizadores(int id, Utilizadores utilizadores)
        {
            if (id != utilizadores.Id)
            {
                return BadRequest();
            }

            _context.Entry(utilizadores).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadoresExists(id))
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

        // POST: api/Utilizadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizadores>> PostUtilizadores(Utilizadores utilizadores)
        {
            _context.Utilizadores.Add(utilizadores);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilizadores", new { id = utilizadores.Id }, utilizadores);
        }

        // DELETE: api/Utilizadores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizadores(int id)
        {
            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            _context.Utilizadores.Remove(utilizadores);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    }
}
