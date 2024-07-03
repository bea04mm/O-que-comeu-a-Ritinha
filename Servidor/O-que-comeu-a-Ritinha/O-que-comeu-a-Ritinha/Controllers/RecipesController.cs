using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using System.Security.Claims;

namespace O_que_comeu_a_Ritinha.Controllers
{
	public class RecipesController : Controller
	{
		private readonly ApplicationDbContext _context;

		private readonly IWebHostEnvironment _webHostEnvironment;

		public RecipesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;

		}

		// GET: Recipes
		public async Task<IActionResult> Index(int? page)
		{
			int pageNumber = page ?? 1; // Se nenhum número de página for fornecido, padrão para a página 1
			int pageSize = 8; // Número de receitas por página

			var recipe = await _context.Recipes
				.OrderBy(r => r.Id)
				.ToPagedListAsync(pageNumber, pageSize); // Uso do ToPagedListAsync para obter a página especificada

			return View(recipe);
		}

		// GET: Recipes/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var recipe = await _context.Recipes
					.Include(r => r.ListIngredients).ThenInclude(ir => ir.Ingredient)
					.Include(r => r.ListTags).ThenInclude(rt => rt.Tag)
					.FirstOrDefaultAsync(m => m.Id == id);

			var favorites = await _context.Recipes
			.Include(r => r.ListUtilizadores)
			.ThenInclude(ru => ru.Utilizador)
			.FirstOrDefaultAsync(r => r.Id == id);

			if (recipe == null)
			{
				return NotFound();
			}

			// Verifica se o utilizador atual está autenticado
			if (User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtém o ID do utilizador atual

				// Busca o ID do Utilizador baseado no userId
				var utilizadorId = await _context.Utilizadores
					.Where(u => u.UserId == userId)
					.Select(u => u.Id)
					.FirstOrDefaultAsync();

				// Verificar se a receita está nos favoritos do utilizador
				var isFavorite = favorites.ListUtilizadores
					.Any(ru => ru.Utilizador.Id == utilizadorId && ru.Recipe.Id == favorites.Id);

				// Devolve os bool para o isFavorite
				ViewBag.IsFavorite = isFavorite;
			}
			else
			{
				ViewBag.IsFavorite = false;
			}

			return View(recipe);
		}

		[Authorize] // Somente utilizadores autenticados podem adicionar aos favoritos
		[HttpPost]
		public async Task<IActionResult> AddToFavorites(int recipeId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtém o ID do utilizador atual

			// Verifica se já existe essa associação na tabela de associação
			var existingAssociation = await _context.RecipesUtilizadores.FirstOrDefaultAsync(ru => ru.RecipeFK == recipeId && ru.Utilizador.UserId == userId);

			if (existingAssociation != null)
			{
				// Já está nos favoritos, então remove
				_context.RecipesUtilizadores.Remove(existingAssociation);
			}
			else
			{
				// Não está nos favoritos, adiciona
				// Encontra o Utilizador pelo UserId
				var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserId == userId);

				// Cria uma nova entrada na tabela de associação
				var newAssociation = new RecipesUtilizadores
				{
					RecipeFK = recipeId,
					UtilizadorFK = utilizador.Id // Utiliza o Id do Utilizador encontrado
				};

				_context.RecipesUtilizadores.Add(newAssociation);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Details", new { id = recipeId });
		}


		[Authorize(Roles = "Admin")]
		// GET: Recipes/Create
		public IActionResult Create()
		{
			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			return View();
		}

		// POST: Recipes/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipe, List<int> Ingredients, List<string> Quantities, List<int> Tags, IFormFile ImageRecipe)
		{
			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			if (ModelState.IsValid)
			{
				string imageName = "";
				bool haImagem = false;

				// há ficheiro?
				if (ImageRecipe == null)
				{
					// não há
					// crio mensagem de erro
					ModelState.AddModelError("", "Deve fornecer uma imagem");
					// devolve controlo à View
					return View(recipe);
				}
				else
				{
					// há ficheiro, mas é uma imagem?
					if (!(ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg"))
					{
						// não
						// vamos usar uma imagem pré-definida
						recipe.Image = "imageRecipe.jpg";
					}
					else
					{
						// há imagem
						haImagem = true;
						// gera nome imagem
						Guid g = Guid.NewGuid();
						imageName = g.ToString();
						string exeImage = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
						imageName += exeImage;
						// guardar nome do ficheiro na BD
						recipe.Image = imageName;

					}
				}

				_context.Add(recipe);
				await _context.SaveChangesAsync();

				// adicionar os ingredientes
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

				// adicionar as tags
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

				// atualiza a receita com os ingredientes e tags associados
				recipe.ListIngredients = listIngredients;
				recipe.ListTags = listTags;

				_context.Update(recipe);
				await _context.SaveChangesAsync();

				// guardar a imagem da receita
				if (haImagem)
				{
					// encolher a imagem ao tamanho certo (api ImageSharp)
					using (var image = Image.Load(ImageRecipe.OpenReadStream()))
					{
						// encolhe para um quadrado
						image.Mutate(x => x.Resize(200, 200));
						// determinar o local de armazenamento da imagem
						string imagePath = _webHostEnvironment.WebRootPath;
						// adicionar à raiz da parte web, o nome da pasta onde queremos guardar a imagem
						imagePath = Path.Combine(imagePath, "images");
						// será que o local existe?
						if (!Directory.Exists(imagePath))
						{
							Directory.CreateDirectory(imagePath);
						}
						// atribuir ao caminho o nome da imagem
						imagePath = Path.Combine(imagePath, imageName);
						// guardar imagem no Disco Rígido
						using (var stream = new FileStream(imagePath, FileMode.Create))
						{
							image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
							await ImageRecipe.CopyToAsync(stream);
						}
					}
				}

				// redireciona o utilizador para a página de 'início' das Recipes
				return RedirectToAction(nameof(Index));
			}
			return View(recipe);
		}

		[Authorize(Roles = "Admin")]
		// GET: Recipes/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var recipe = await _context.Recipes
					.Include(r => r.ListIngredients).ThenInclude(ir => ir.Ingredient)
					.Include(r => r.ListTags).ThenInclude(rt => rt.Tag)
					.FirstOrDefaultAsync(m => m.Id == id);

			if (recipe == null)
			{
				return NotFound();
			}

			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			return View(recipe);
		}

		// POST: Recipes/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipe, List<int> Ingredients, List<string> Quantities, List<int> Tags, IFormFile ImageRecipe, string CurrentImageName)
		{
			if (id != recipe.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					string imageName = CurrentImageName;
					bool haImagem = false;

					if (ImageRecipe != null)
					{
						// Nova imagem dada
						if (!(ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg"))
						{
							recipe.Image = "imageRecipe.jpg";
						}
						else
						{
							haImagem = true;
							Guid g = Guid.NewGuid();
							imageName = g.ToString();
							string exeImage = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
							imageName += exeImage;
							recipe.Image = imageName;
						}

						// Remove a imagem antiga se existe
						if (!string.IsNullOrEmpty(CurrentImageName) && CurrentImageName != "imageRecipe.jpg")
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
						// Não há nova imagem, continua com a antiga
						recipe.Image = CurrentImageName;
					}

					// Update dos detalhes das receitas
					_context.Update(recipe);
					await _context.SaveChangesAsync();

					// Update dos ingredientes e tags
					var existingIngredients = _context.IngredientsRecipes.Where(ir => ir.RecipeFK == id).ToList();
					_context.IngredientsRecipes.RemoveRange(existingIngredients);
					var existingTags = _context.RecipesTags.Where(rt => rt.RecipeFK == id).ToList();
					_context.RecipesTags.RemoveRange(existingTags);

					await _context.SaveChangesAsync();

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

					recipe.ListIngredients = listIngredients;
					recipe.ListTags = listTags;

					_context.Update(recipe);
					await _context.SaveChangesAsync();

					// Guarda a nova imagem se dada
					if (haImagem)
					{
						using (var image = Image.Load(ImageRecipe.OpenReadStream()))
						{
							image.Mutate(x => x.Resize(200, 200));
							string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
							using (var stream = new FileStream(imagePath, FileMode.Create))
							{
								image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
								await ImageRecipe.CopyToAsync(stream);
							}
						}
					}
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RecipesExists(recipe.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}

			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			return View(recipe);
		}

		[Authorize(Roles = "Admin")]
		// GET: Recipes/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

			if (recipe == null)
			{
				return NotFound();
			}

			return View(recipe);
		}

		// POST: Recipes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var recipe = await _context.Recipes.FindAsync(id);

			if (recipe != null)
			{
				_context.Recipes.Remove(recipe);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool RecipesExists(int id)
		{
			return _context.Recipes.Any(e => e.Id == id);
		}
	}
}
