using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.File.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        private readonly IGenericRepository<ProductImageEntity> _repo;

        public FileController(IGenericRepository<ProductImageEntity> repo)
        {
            _repo = repo;
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int productId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var filePath = Path.Combine(_storagePath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var existProduct = await _repo.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (existProduct != null)
            {
                existProduct.Url = filePath;
                await _repo.Update(existProduct);
                return Ok(new { message = "Dosya güncellendi.", fileName = file.FileName });
            }

            await _repo.Add(new ProductImageEntity
            {
                ProductId = productId,
                Url = filePath,
                CreatedAt = DateTime.Now
            });

            return Ok(new { message = "Dosya yüklendi.", fileName = file.FileName });
        }

        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            System.IO.File.Delete(filePath);
            var file = await _repo.FirstOrDefaultAsync(p => p.Url == filePath);
            
            if (file == null)
                return NotFound("Veritabanında dosya bulunamadı.");

            //Veritabanından dosyayı silmeden önce product kullanılıyormu actifmi gibi bir kontrol daha gerekli diye düşünüyorum fakat su an direk silme işlemini gerçekleştiriyorum.

            await _repo.Delete(file.Id);

            return Ok(new { message = "Dosya silindi.", fileName });
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllFiles()
        {
            // burdaki yaklaşımdaki amaç seed sahte dalar hazırlanırken eklenen dosyaların url lerini de dosya sistemimize eklemek amaçlanmıştır.Farklı dosya konumu işaret eden dataları da eklemektir.
            var productImages = await _repo.GetAllAsync();
            var fileUrls = productImages.Select(p => p.Url).ToList();
            var files = Directory.GetFiles(_storagePath);
            var fileNames = files.Select(Path.GetFileName).ToList();
            fileNames.AddRange(fileUrls);
            return Ok(fileNames);
        }
        [HttpGet("productFiles")]
        public async Task<IActionResult> GetProductFiles(int productId)
        {
            var productImages = await _repo.GetAllIncludingAsync(p => p.Product.Id == productId);

            if (productImages == null || !productImages.Any())
                return NotFound("Ürüne ait dosya bulunamadı.");
            var productFileUrls = productImages.Select(p => p.Url).ToList();

            return Ok(productFileUrls);
        }
    }
}
