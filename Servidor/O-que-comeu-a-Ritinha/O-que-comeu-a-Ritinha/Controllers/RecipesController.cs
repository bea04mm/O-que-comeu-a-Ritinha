using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Migrations;
using O_que_comeu_a_Ritinha.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

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
        public async Task<IActionResult> Create([Bind("Title,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipes, List<int> Ingredients, List<string> Quantities, List<int> Tags, IFormFile ImageRecipe)
		{
            ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			if (ModelState.IsValid)
            {
				string nomeImagem = "";
                bool haImagem = false;

				// há ficheiro?
				if (ImageRecipe == null)
				{
					// não há
					// crio mensagem de erro
					ModelState.AddModelError("", "Deve fornecer uma imagem");
					// devolve controlo à View
					return View(recipes);
				}
				else
				{
					// há ficheiro, mas é uma imagem?
					if (!(ImageRecipe.ContentType == "image/png" || ImageRecipe.ContentType == "image/jpeg"))
					{
						// não
						// vamos usar uma imagem pré-definida
						recipes.Image = "imageRecipe.jpg";
					}
					else
					{
						// há imagem
						haImagem = true;
						// gera nome imagem
						Guid g = Guid.NewGuid();
						nomeImagem = g.ToString();
						string extensaoImagem = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
						nomeImagem += extensaoImagem;
						// guardar nome do ficheiro na BD
						recipes.Image = nomeImagem;

					}
				}

				_context.Add(recipes);
                await _context.SaveChangesAsync();

				// adicionar os ingredientes
				List<IngredientsRecipes> listIngredients = new List<IngredientsRecipes>();
				for (int i = 0; i < Ingredients.Count; i++)
				{
					IngredientsRecipes ingredientsRecipes = new IngredientsRecipes
					{
						IngredientFK = Ingredients[i],
						RecipeFK = recipes.Id,
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
						RecipeFK = recipes.Id
					};

					listTags.Add(recipesTags);
				}

				recipes.ListIngredients = listIngredients;
				recipes.ListTags = listTags;

				_context.Update(recipes);
				await _context.SaveChangesAsync();

				// guardar a imagem do logótipo
				if (haImagem)
				{
					// encolher a imagem ao tamanho certo --> fazer por mim (NuGet resize image)

					// determinar o local de armazenamento da imagem
					string localizacaoImagem = _webHostEnvironment.WebRootPath;
					// adicionar à raiz da parte web, o nome da pasta onde queremos guardar a imagem
					localizacaoImagem = Path.Combine(localizacaoImagem, "images");
					// será que o local existe?
					if (!Directory.Exists(localizacaoImagem))
					{
						Directory.CreateDirectory(localizacaoImagem);
					}
					// atribuir ao caminho o nome da imagem
					localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);
					// guardar imagem no Disco Rígido
					using var stream = new FileStream(localizacaoImagem, FileMode.Create);
					await ImageRecipe.CopyToAsync(stream);
				}

				// redireciona o utilizador para a página de 'início' das Recipes
				return RedirectToAction(nameof(Index));
            }
            return View(recipes);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var recipes = await _context.Recipes.Include(r => r.ListIngredients).ThenInclude(ir => ir.Ingredient).FirstOrDefaultAsync(m => m.Id == id);

			if (recipes == null)
			{
				return NotFound();
			}

			ViewData["ListIngredients"] = new SelectList(_context.Ingredients, "Id", "Ingredient");
			ViewData["ListTags"] = new SelectList(_context.Tags, "Id", "Tag");

			return View(recipes);
		}

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Image,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipes, List<int> Ingredients, List<string> Quantities, List<int> Tags)
        {
            if (id != recipes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipes);
                    await _context.SaveChangesAsync();

					var existingIngredients = _context.IngredientsRecipes.Where(ir => ir.RecipeFK == id).ToList();
					_context.IngredientsRecipes.RemoveRange(existingIngredients);
					var existingTags = _context.RecipesTags.Where(ir => ir.RecipeFK == id).ToList();
					_context.RecipesTags.RemoveRange(existingTags);

					await _context.SaveChangesAsync();

					List<IngredientsRecipes> listIngredients = new List<IngredientsRecipes>();
                    for (int i = 0; i < Ingredients.Count; i++)
                    {
                        IngredientsRecipes ingredientsRecipes = new IngredientsRecipes
                        {
                            IngredientFK = Ingredients[i],
                            RecipeFK = recipes.Id,
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
							RecipeFK = recipes.Id,
						};

						listTags.Add(recipesTags);
					}

					recipes.ListIngredients = listIngredients;
					recipes.ListTags = listTags;

					_context.Update(recipes);
					await _context.SaveChangesAsync();
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipesExists(recipes.Id))
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

			return View(recipes);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipes = await _context.Recipes.FindAsync(id);
            if (recipes != null)
            {
                _context.Recipes.Remove(recipes);
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
