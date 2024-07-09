using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using System.Security.Claims;

namespace O_que_comeu_a_Ritinha.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipesUtilizadores
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.RecipesUtilizadores
                .Include(r => r.Recipe)
                .Include(r => r.Utilizador)
                .Where(r => r.Utilizador.UserId == userId); // Filter by authenticated user
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipesUtilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipesUtilizadores = await _context.RecipesUtilizadores
                .Include(r => r.Recipe)
                .Include(r => r.Utilizador)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipesUtilizadores == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (recipesUtilizadores.Utilizador.UserId != userId)
            {
                return Forbid();
            }

            return View(recipesUtilizadores);
        }

        // POST: RecipesUtilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipesUtilizadores = await _context.RecipesUtilizadores.FindAsync(id);

            if (recipesUtilizadores == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (recipesUtilizadores.Utilizador.UserId != userId)
            {
                return Forbid();
            }

            _context.RecipesUtilizadores.Remove(recipesUtilizadores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipesUtilizadoresExists(int id)
        {
            return _context.RecipesUtilizadores.Any(e => e.Id == id);
        }
    }
}
