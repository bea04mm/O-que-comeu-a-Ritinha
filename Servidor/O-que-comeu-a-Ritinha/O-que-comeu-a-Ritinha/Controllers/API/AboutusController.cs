using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AboutusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AboutusController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
			_webHostEnvironment = webHostEnvironment;
		}

        // GET: api/Aboutus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aboutus>>> GetAboutus()
        {
            return await _context.Aboutus.ToListAsync();
        }

        // GET: api/Aboutus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aboutus>> GetAboutus(int id)
        {
            var aboutus = await _context.Aboutus.FindAsync(id);

            if (aboutus == null)
            {
                return NotFound();
            }

            return aboutus;
        }

		// PUT: api/Aboutus/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("PutAboutus/{id}")]
		public async Task<IActionResult> PutAboutus(int id, [FromForm] Aboutus aboutus, [FromForm] List<int> Recipes, IFormFile ImageDescription, IFormFile ImageLogo, [FromForm] string CurrentImageDescription, [FromForm] string CurrentImageLogo)
		{
			if (id != aboutus.Id)
			{
				return BadRequest("ID mismatch.");
			}

			if (Recipes.Count < 3 || (CurrentImageDescription == null && ImageDescription == null) || (CurrentImageLogo == null && ImageLogo == null))
			{
				return BadRequest("Please provide at least 3 recipes and both images.");
			}

			// Process images
			aboutus.ImageDescription = await ProcessImage(ImageDescription, CurrentImageDescription, false);
			aboutus.ImageLogo = await ProcessImage(ImageLogo, CurrentImageLogo, true);

			// Update Aboutus entity
			_context.Entry(aboutus).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();

				// Update associated recipes
				var existingRecipes = _context.AboutusRecipes.Where(r => r.AboutusFK == id).ToList();
				_context.AboutusRecipes.RemoveRange(existingRecipes);
				await _context.SaveChangesAsync();

				List<AboutusRecipes> listRecipes = Recipes.Select(recipeId => new AboutusRecipes
				{
					AboutusFK = aboutus.Id,
					RecipeFK = recipeId
				}).ToList();

				await _context.AboutusRecipes.AddRangeAsync(listRecipes);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AboutusExists(id))
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
