using dipSecSerAPI1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dipSecSerAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleriesController : ControllerBase
    {
        private readonly DipSecSerContext _context;
        private readonly string _imageFolderPath;

        public GalleriesController(DipSecSerContext context, IWebHostEnvironment env)
        {
            _context = context;

            _imageFolderPath = Path.Combine(env.WebRootPath, "images");

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл не был загружен");
            }

            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");

            try
            {
                // Генерация уникального имени изображения
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(imageDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var galleryItem = new Gallery
                {
                    Name = file.Name,
                    Description = "Описание изображение",
                    ImagePath = $"/images/{fileName}",

                };

                _context.Galleries.Add(galleryItem);
                await _context.SaveChangesAsync();

                return Ok(new {imagePath = galleryItem.ImagePath});


            }
            catch (IOException ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
    }
}
