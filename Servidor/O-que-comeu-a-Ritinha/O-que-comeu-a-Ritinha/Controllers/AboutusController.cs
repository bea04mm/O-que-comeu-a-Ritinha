using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using O_que_comeu_a_Ritinha.Migrations;

namespace O_que_comeu_a_Ritinha.Controllers
{
	public class AboutusController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AboutusController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Aboutus
		public async Task<IActionResult> Index()
		{
			return View(await _context.Aboutus.ToListAsync());
		}

		[Authorize(Roles = "Admin")]
		// GET: Aboutus/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var aboutus = await _context.Aboutus
					.Include(ra => ra.ListRecipesA).ThenInclude(r => r.Recipe)
					.FirstOrDefaultAsync(m => m.Id == id);

			if (aboutus == null)
			{
				return NotFound();
			}

			ViewData["ListRecipesA"] = new SelectList(_context.Recipes.OrderBy(r => r.Title), "Id", "Title");


			return View(aboutus);
		}

		// POST: Aboutus/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] Aboutus aboutus, List<int> Recipes, IFormFile ImageDescription, IFormFile ImageLogo, string CurrentImageDescription, string CurrentImageLogo)
		{
			if (id != aboutus.Id)
			{
				return NotFound();
			}

			ViewData["ListRecipesA"] = new SelectList(_context.Recipes, "Id", "Title");

			if (Recipes.Count < 3 || (CurrentImageDescription == null && ImageDescription == null) || (CurrentImageLogo == null && ImageLogo == null))
			{
				ModelState.AddModelError("", "Por favor, adicione pelo 3 receitas e as duas imagens. Para reverter o que foi apagado volta à lista e volta a editar esta página.");
				return View(aboutus);
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Processa images
					aboutus.ImageDescription = await ProcessImage(ImageDescription, CurrentImageDescription, false);
					aboutus.ImageLogo = await ProcessImage(ImageLogo, CurrentImageLogo, true);

					_context.Update(aboutus);
					await _context.SaveChangesAsync();

					var existingRecipes = _context.AboutusRecipes.Where(r => r.AboutusFK == id).ToList();
					_context.AboutusRecipes.RemoveRange(existingRecipes);

					await _context.SaveChangesAsync();

					List<AboutusRecipes> listRecipes = new List<AboutusRecipes>();
					for (int i = 0; i < Recipes.Count; i++)
					{
						AboutusRecipes aboutRecipes = new AboutusRecipes
						{
							AboutusFK = aboutus.Id,
							RecipeFK = Recipes[i]
						};
						listRecipes.Add(aboutRecipes);
					}

					aboutus.ListRecipesA = listRecipes;

					_context.Update(aboutus);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AboutusExists(aboutus.Id))
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
			ViewData["ListRecipesA"] = new SelectList(_context.Recipes, "Id", "Title");

			return View(aboutus);
		}

		private async Task<string> ProcessImage(IFormFile imageFile, string currentImageName, bool isImageLogo)
		{
			return await Task.Run(() =>
			{
				// ha ficheiro?
				if (imageFile != null)
				{
					// ha ficheiro, mas e uma imagem?
					if (imageFile.ContentType == "image/png" || imageFile.ContentType == "image/jpeg")
					{
						// Determina o nome da imagem
						string imageName;
						if (isImageLogo)
						{
							// Para ImageLogo, guarda sempre como "imageLogo.png"
							imageName = "imageLogo.png";
						}
						else
						{
							// Gera um nome unico baseado no GUID e na estensao do ficheiro original
							string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
							imageName = $"{Guid.NewGuid()}{fileExtension}";
						}

						string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);

						// Guarda a nova imagem
						using (var image = Image.Load(imageFile.OpenReadStream()))
						{
							image.Mutate(x => x.Resize(200, 200));
							using (var stream = new FileStream(imagePath, FileMode.Create))
							{
								image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
							}
						}

						// Retira a imagem antiga, se existe e nao e a default
						if (!string.IsNullOrEmpty(currentImageName) && currentImageName != "imageLogo.png" && !isImageLogo)
						{
							var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", currentImageName);
							if (System.IO.File.Exists(oldImagePath))
							{
								System.IO.File.Delete(oldImagePath);
							}
						}

						return imageName;
					}
					else
					{
						ModelState.AddModelError("", "Formato Inválido da Imagem. Por Favor faz upload de um ficheiro PNG ou JPEG.");
						return currentImageName;
					}
				}

				// Devolve a imagem atual se nao for dada imagem nova
				return currentImageName;
			});
		}


		private bool AboutusExists(int id)
		{
			return _context.Aboutus.Any(e => e.Id == id);
		}
	}
}
