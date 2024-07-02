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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using Microsoft.AspNetCore.Authorization;

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
					.Include(r => r.ListIngredients).ThenInclude(ir => ir.Ingredient)
					.Include(r => r.ListTags).ThenInclude(rt => rt.Tag)
					.FirstOrDefaultAsync(m => m.Id == id);

			if (recipes == null)
            {
                return NotFound();
            }

            return View(recipes);
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
        public async Task<IActionResult> Create([Bind("Title,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipes, List<int> Ingredients, List<string> Quantities, List<int> Tags, IFormFile ImageRecipe)
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
                        imageName = g.ToString();
                        string exeImage = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
                        imageName += exeImage;
                        // guardar nome do ficheiro na BD
                        recipes.Image = imageName;

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

                // atualiza a receita com os ingredientes e tags associados
                recipes.ListIngredients = listIngredients;
                recipes.ListTags = listTags;

                _context.Update(recipes);
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
            return View(recipes);
        }

        [Authorize(Roles = "Admin")]
        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes
                    .Include(r => r.ListIngredients).ThenInclude(ir => ir.Ingredient)
                    .Include(r => r.ListTags).ThenInclude(rt => rt.Tag)
                    .FirstOrDefaultAsync(m => m.Id == id);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Time,Portions,Suggestions,Instagram,Steps")] Recipes recipes, List<int> Ingredients, List<string> Quantities, List<int> Tags, IFormFile ImageRecipe, string CurrentImageName)
        {
            if (id != recipes.Id)
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
							recipes.Image = "imageRecipe.jpg";
						}
						else
					    {
                            haImagem = true;
							Guid g = Guid.NewGuid();
							imageName = g.ToString();
							string exeImage = Path.GetExtension(ImageRecipe.FileName).ToLowerInvariant();
							imageName += exeImage;
							recipes.Image = imageName;
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
                        recipes.Image = CurrentImageName;
                    }

					// Update dos detalhes das receitas
					_context.Update(recipes);
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
							RecipeFK = recipes.Id
						};
						listTags.Add(recipesTags);
					}

					recipes.ListIngredients = listIngredients;
					recipes.ListTags = listTags;

					_context.Update(recipes);
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

        [Authorize(Roles = "Admin")]
        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipes = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);

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
