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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

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

		[HttpGet("GetPagedRecipes")]
		public async Task<ActionResult> GetPagedRecipes([FromQuery] int page = 1, [FromQuery] string searchString = "")
		{
			int pageSize = 8;
			var recipesQuery = _context.Recipes
				.Include(r => r.ListTags)
				.AsQueryable();

			if (!string.IsNullOrEmpty(searchString))
			{
				recipesQuery = recipesQuery.Where(r => r.Title.Contains(searchString) || r.ListTags.Any(rt => rt.Tag.Tag.Contains(searchString)));
			}

			var totalRecipes = await recipesQuery.CountAsync(); // Get the total number of recipes matching the query
			var totalPages = (int)Math.Ceiling(totalRecipes / (double)pageSize); // Calculate total pages

			var pagedRecipes = await recipesQuery
				.OrderBy(r => r.Title)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var response = new
			{
				Recipes = pagedRecipes,
				TotalPages = totalPages
			};

			return Ok(response); // Return both the recipes and total pages
		}

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

		[HttpPost("AddToFavoritesReact")]
		public async Task<IActionResult> AddToFavoritesReact([FromBody] int recipeId)
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
		public async Task<IActionResult> PutRecipes(int id, [FromForm] Recipes recipe, [FromForm] List<int> Ingredients, [FromForm] List<string> Quantities, [FromForm] List<int> Tags, [FromForm] IFormFile ImageRecipe, [FromForm] string CurrentImageName)
		{
			if (id != recipe.Id)
			{
				return BadRequest("Recipe ID mismatch.");
			}

			// Check if the provided data is valid
			if (Ingredients.Count == 0 || Tags.Count == 0 || Quantities.Any(q => string.IsNullOrWhiteSpace(q)) || (CurrentImageName == null && ImageRecipe == null))
			{
				return BadRequest("Please provide at least one ingredient with quantity, one tag, and an image.");
			}

			try
			{
				// Handle image update
				string imageName = CurrentImageName;
				bool haImagem = false;

				// Check if a new image has been uploaded
				if (ImageRecipe != null)
				{
					if (ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg")
					{
						haImagem = true;
						Guid g = Guid.NewGuid();
						imageName = g.ToString();
						string exeImage = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
						imageName += exeImage;
						recipe.Image = imageName;
					}
					else
					{
						recipe.Image = "imageRecipe.png"; // Default image
					}

					// Delete the old image if a new one has been provided
					if (!string.IsNullOrEmpty(CurrentImageName) && CurrentImageName != "imageRecipe.png")
					{
						var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", CurrentImageName);
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
				}
				else
				{
					// Retain the old image if no new image is uploaded
					recipe.Image = CurrentImageName;
				}

				// Update recipe details
				_context.Entry(recipe).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				// Update ingredients and tags
				var existingIngredients = _context.IngredientsRecipes.Where(ir => ir.RecipeFK == id).ToList();
				_context.IngredientsRecipes.RemoveRange(existingIngredients);

				var existingTags = _context.RecipesTags.Where(rt => rt.RecipeFK == id).ToList();
				_context.RecipesTags.RemoveRange(existingTags);

				await _context.SaveChangesAsync();

				// Add new ingredients
				List<IngredientsRecipes> listIngredients = new List<IngredientsRecipes>();
				for (int i = 0; i < Ingredients.Count; i++)
				{
					IngredientsRecipes ingredientsRecipes = new IngredientsRecipes
					{
						IngredientFK = Ingredients[i],
						RecipeFK = recipe.Id,
						Quantity = Quantities[i]
					};
					listIngredients.Add(ingredientsRecipes);
				}

				// Add new tags
				List<RecipesTags> listTags = new List<RecipesTags>();
				for (int i = 0; i < Tags.Count; i++)
				{
					RecipesTags recipesTags = new RecipesTags
					{
						TagFK = Tags[i],
						RecipeFK = recipe.Id
					};
					listTags.Add(recipesTags);
				}

				// Save ingredients and tags
				_context.IngredientsRecipes.AddRange(listIngredients);
				_context.RecipesTags.AddRange(listTags);
				await _context.SaveChangesAsync();

				// Save the new image if provided
				if (haImagem)
				{
					using (var image = Image.Load(ImageRecipe.OpenReadStream()))
					{
						image.Mutate(x => x.Resize(200, 200));
						string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
						using (var stream = new FileStream(imagePath, FileMode.Create))
						{
							image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
						}
					}
				}
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
		[HttpPost("PostRecipes")]
		public async Task<ActionResult<Recipes>> PostRecipes([FromForm] Recipes recipe, [FromForm] List<int> Ingredients, [FromForm] List<string> Quantities, [FromForm] List<int> Tags, IFormFile ImageRecipe) {
			string imageName = "";
			bool haImagem = false;

			// Handle image upload
			if (ImageRecipe != null)
			{
				// Check if the uploaded file is a valid image
				if (ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg")
				{
					haImagem = true;
					// Generate a unique name for the image
					Guid g = Guid.NewGuid();
					imageName = g.ToString() + Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
					recipe.Image = imageName;
				}
				else
				{
					// Assign a default image if the uploaded file is not valid
					recipe.Image = "imageRecipe.png";
				}
			}

			// Add the recipe to the database
			_context.Recipes.Add(recipe);
			await _context.SaveChangesAsync();

			// Add ingredients and quantities
			List<IngredientsRecipes> listIngredients = new List<IngredientsRecipes>();
			for (int i = 0; i < Ingredients.Count; i++)
			{
				IngredientsRecipes ingredientsRecipes = new IngredientsRecipes
				{
					IngredientFK = Ingredients[i],
					RecipeFK = recipe.Id,
					Quantity = Quantities[i]
				};
				listIngredients.Add(ingredientsRecipes);
			}

			// Add tags
			List<RecipesTags> listTags = new List<RecipesTags>();
			for (int i = 0; i < Tags.Count; i++)
			{
				RecipesTags recipesTags = new RecipesTags
				{
					TagFK = Tags[i],
					RecipeFK = recipe.Id
				};
				listTags.Add(recipesTags);
			}

			// Update recipe with associated ingredients and tags
			recipe.ListIngredients = listIngredients;
			recipe.ListTags = listTags;
			_context.Recipes.Update(recipe);
			await _context.SaveChangesAsync();

			// Save image to disk
			if (haImagem)
			{
				string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
				if (!Directory.Exists(imagePath))
				{
					Directory.CreateDirectory(imagePath);
				}
				imagePath = Path.Combine(imagePath, imageName);

				using (var stream = new FileStream(imagePath, FileMode.Create))
				{
					using (var image = Image.Load(ImageRecipe.OpenReadStream()))
					{
						image.Mutate(x => x.Resize(200, 200));
						image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
					}
					await ImageRecipe.CopyToAsync(stream);
				}
			}

			return CreatedAtAction("GetRecipes", new { id = recipe.Id }, recipe);
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
