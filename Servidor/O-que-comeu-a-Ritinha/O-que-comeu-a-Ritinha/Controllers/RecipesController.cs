using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using X.PagedList.Extensions;

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
		public async Task<IActionResult> Index(int? page, string searchString)
		{
			int pageNumber = page ?? 1; // Se nenhum numero de pagina for fornecido, padrao para a página 1
			int pageSize = 8; // Numero de receitas por pagina

            if (string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = null;
            }
            else
            {
                ViewBag.CurrentFilter = searchString;
            }

            // Obter todas as receitas
            var recipes = _context.Recipes
				.Include(r => r.ListTags)
				.AsQueryable();

			// Filtrar por titulo se o searchString nao estiver vazio
			if (!String.IsNullOrEmpty(searchString))
			{
				recipes = recipes.Where(r => r.Title.Contains(searchString) || r.ListTags.Any(rt => rt.Tag.Tag.Contains(searchString)));
			}

			var pagedRecipes = recipes.OrderBy(r => r.Title)
				.ToPagedList(pageNumber, pageSize); // Uso do ToPagedListAsync para obter a pagina especificada

            return View(pagedRecipes);
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

			// Verifica se o utilizador atual esta autenticado
			if (User.Identity.IsAuthenticated)
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Obtem o ID do utilizador atual

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

			// Verifica se ja existe essa associacao na tabela de associacao
			var existingAssociation = await _context.Favorites.FirstOrDefaultAsync(ru => ru.RecipeFK == recipeId && ru.Utilizador.UserId == userId);

			if (existingAssociation != null)
			{
				// Ja esta nos favoritos, entao remove
				_context.Favorites.Remove(existingAssociation);
			}
			else
			{
				// Nao esta nos favoritos, adiciona
				// Encontra o Utilizador pelo UserId
				var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.UserId == userId);

				// Cria uma nova entrada na tabela de associacao
				var newAssociation = new Favorites
				{
					RecipeFK = recipeId,
					UtilizadorFK = utilizador.Id // Utiliza o Id do Utilizador encontrado
				};

				_context.Favorites.Add(newAssociation);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Details", new { id = recipeId });
		}


		[Authorize(Roles = "Admin")]
		// GET: Recipes/Create
		public IActionResult Create()
		{
			ViewData["ListIngredients"] = new SelectList(_context.Ingredients.OrderBy(i => i.Ingredient), "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags.OrderBy(t => t.Tag), "Id", "Tag");

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

				if (Ingredients.Count == 0 || Tags.Count == 0 || ImageRecipe == null || Quantities.Any(q => string.IsNullOrWhiteSpace(q)))
				{
					ModelState.AddModelError("", "Por favor, adicione pelo menos um ingrediente e a sua quantidade, uma tag e uma imagem.");
					return View(recipe);
				}

				// ha ficheiro?
				if (ImageRecipe != null)
				{
					// ha ficheiro, mas é uma imagem?
					if (!(ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg"))
					{
						// não
						// vamos usar uma imagem pre-definida
						recipe.Image = "imageRecipe.png";
					}
					else
					{
						// ha imagem
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

				// adicionar os ingredientes e quantidades
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
						// adicionar a raiz da parte web, o nome da pasta onde queremos guardar a imagem
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

				// redireciona o utilizador para a pagina de 'inicio' das Recipes
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

			ViewBag.ListIngredients = recipe.ListIngredients.Select(ir => new { ir.Ingredient.Id, ir.Ingredient.Ingredient, ir.Quantity }).ToList();
			ViewBag.ListTags = recipe.ListTags.Select(rt => new { rt.Tag.Id, rt.Tag.Tag }).ToList();

			ViewData["ListIngredients"] = new SelectList(_context.Ingredients.OrderBy(i => i.Ingredient), "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags.OrderBy(t => t.Tag), "Id", "Tag");

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

			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			if (Ingredients.Count == 0 || Tags.Count == 0 || Quantities.Any(q => string.IsNullOrWhiteSpace(q)) || (CurrentImageName == null && ImageRecipe == null))
			{
				ModelState.AddModelError("", "Por favor, adicione pelo menos um ingrediente e a sua quantidade, uma tag e uma imagem. Para reverter o que foi apagado volta à lista e volta a editar esta receita.");
				return View(recipe);
			}

			if (ModelState.IsValid)
			{
				try
				{
					string imageName = CurrentImageName;
					bool haImagem = false;

					// ha ficheiro?
					if (ImageRecipe != null)
					{
						// Nova imagem dada
						if (!(ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg"))
						{
							recipe.Image = "imageRecipe.png";
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
				// Caminho da imagem a ser apagada
				string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", recipe.Image);

				// Remove a receita
				_context.Recipes.Remove(recipe);

				// Apaga a imagem fisica do servidor, se nao for a imagem padrao
				if (recipe.Image != "imageRecipe.png" && System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
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
