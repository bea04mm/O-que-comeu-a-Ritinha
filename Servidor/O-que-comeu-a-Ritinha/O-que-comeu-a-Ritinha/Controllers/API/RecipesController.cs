using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public RecipesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: api/Recipes
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Recipes>>> GetRecipes()
		{
			return await _context.Recipes
				.OrderBy(r => r.Title)
				.ToListAsync();
		}
		//public async Task<ActionResult<IEnumerable<Recipes>>> GetPagedRecipes([FromQuery] int page = 1, [FromQuery] string searchString = "")
		//{
		//	int pageSize = 8;
		//	var recipesQuery = _context.Recipes
		//		.Include(r => r.ListTags)
		//		.AsQueryable();

		//	if (!string.IsNullOrEmpty(searchString))
		//	{
		//		recipesQuery = recipesQuery.Where(r => r.Title.Contains(searchString) || r.ListTags.Any(rt => rt.Tag.Tag.Contains(searchString)));
		//	}

		//	var pagedRecipes = await recipesQuery
		//		.OrderBy(r => r.Title)
		//		.Skip((page - 1) * pageSize)
		//		.Take(pageSize)
		//		.ToListAsync();

		//	return Ok(pagedRecipes);
		//}

		// GET: api/Recipes/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Recipes>> GetRecipes(int? id)
		{
			var recipe = await _context.Recipes
				.FirstOrDefaultAsync(m => m.Id == id);

			if (recipe == null)
			{
				return NotFound();
			}

			return Ok(recipe);
		}

		//[Authorize] // Somente utilizadores autenticados podem adicionar aos favoritos
		[HttpPost("AddToFavorites")]
		public async Task<IActionResult> AddToFavorites([FromBody] int recipeId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var existingAssociation = await _context.Favorites.FirstOrDefaultAsync(ru => ru.RecipeFK == recipeId && ru.Utilizador.UserId == userId);

			if (existingAssociation != null)
			{
				_context.Favorites.Remove(existingAssociation);
			}
			else
			{
				var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserId == userId);
				var newAssociation = new Favorites
				{
					RecipeFK = recipeId,
					UtilizadorFK = utilizador.Id
				};

				_context.Favorites.Add(newAssociation);
			}

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// PUT: api/PutRecipes/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("PutRecipes/{id}")]
		public async Task<IActionResult> PutRecipes(int id, [FromBody] Recipes recipe)
		{
			if (id != recipe.Id)
			{
				return BadRequest("Recipe ID mismatch.");
			}

			_context.Entry(recipe).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!RecipesExists(id))
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

		// POST: api/PostRecipes
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("PostRecipes")]
		public async Task<ActionResult<Recipes>> PostRecipes([FromBody] Recipes recipe)
		{
			_context.Recipes.Add(recipe);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetRecipes), new { id = recipe.Id }, recipe);
		}

		// DELETE: api/DeleteRecipes/5
		[HttpDelete("DeleteRecipes/{id}")]
        public async Task<IActionResult> DeleteRecipes(int id)
        {
			var recipe = await _context.Recipes.FindAsync(id);
			if (recipe == null)
			{
				return NotFound();
			}

			_context.Recipes.Remove(recipe);
			await _context.SaveChangesAsync();

			// Remove image if it exists and is not the default image
			if (!string.IsNullOrEmpty(recipe.Image) && recipe.Image != "imageRecipe.png")
			{
				var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", recipe.Image);
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
			}

			return NoContent();
		}

        private bool RecipesExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
