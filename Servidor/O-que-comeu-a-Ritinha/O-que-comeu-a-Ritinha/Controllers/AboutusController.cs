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

			ViewData["ListRecipesA"] = new SelectList(_context.Recipes, "Id", "Title");

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
					// Process images
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
				// há ficheiro?
				if (imageFile != null)
				{
					// há ficheiro, mas é uma imagem?
					if (imageFile.ContentType == "image/png" || imageFile.ContentType == "image/jpeg")
					{
						// Determine the image name
						string imageName;
						if (isImageLogo)
						{
							// For ImageLogo, always save as "imageLogo.png"
							imageName = "imageLogo.png";
						}
						else
						{
							// Generate a unique filename based on GUID and original file extension
							string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
							imageName = $"{Guid.NewGuid()}{fileExtension}";
						}

						string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);

						// Save the new image
						using (var image = Image.Load(imageFile.OpenReadStream()))
						{
							image.Mutate(x => x.Resize(200, 200));
							using (var stream = new FileStream(imagePath, FileMode.Create))
							{
								image.SaveAsJpeg(stream, new JpegEncoder { Quality = 100 });
							}
						}

						// Remove the old image if it exists and is not the default
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
						ModelState.AddModelError("", "Invalid image format. Please upload a PNG or JPEG file.");
						return currentImageName;
					}
				}

				// Return the current image name if no new image is provided
				return currentImageName;
			});
		}


		private bool AboutusExists(int id)
		{
			return _context.Aboutus.Any(e => e.Id == id);
		}
	}
}
