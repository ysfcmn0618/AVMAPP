using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

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
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] int productId)
        {
            if (productId <= 0)
                return BadRequest("Geçersiz ürün kimliği.");
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var safeFileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid():N}_{safeFileName}";
            var filePath = Path.Combine(_storagePath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var existProduct = await _repo.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (existProduct != null)
            {
                existProduct.Url = filePath;
                existProduct.UpdatedAt = DateTime.UtcNow;
                await _repo.Update(existProduct);
                return Ok(new { message = "Dosya güncellendi.", fileName = Path.GetFileName(filePath) });
            }

            await _repo.Add(new ProductImageEntity
            {
                ProductId = productId,
                Url = filePath,
                CreatedAt = DateTime.UtcNow
            });

            return Ok(new { message = "Dosya yüklendi.", fileName = Path.GetFileName(filePath) });
        }

        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var safeFileName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_storagePath, safeFileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            return PhysicalFile(filePath, "application/octet-stream", safeFileName);
        }
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var safeFileName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_storagePath, safeFileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            System.IO.File.Delete(filePath);
            var file = await _repo.FirstOrDefaultAsync(p => p.Url == filePath);

            if (file == null)
                return NotFound("Veritabanında dosya bulunamadı.");

            //Veritabanından dosyayı silmeden önce product kullanılıyormu actifmi gibi bir kontrol daha gerekli diye düşünüyorum fakat su an direk silme işlemini gerçekleştiriyorum.

            await _repo.Delete(file.Id);

            return Ok(new { message = "Dosya silindi.", fileName = safeFileName });
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllFiles()
        {
            // burdaki yaklaşımdaki amaç seed sahte dalar hazırlanırken eklenen dosyaların url lerini de dosya sistemimize eklemek amaçlanmıştır.Farklı dosya konumu işaret eden dataları da eklemektir.
            var productImages = await _repo.GetAllAsync();
            var dbNames = productImages
                .Select(p => Path.GetFileName(p.Url))
                .Where(n => !string.IsNullOrWhiteSpace(n));
            var diskNames = Directory.EnumerateFiles(_storagePath).Select(Path.GetFileName);
            var merged = dbNames.Concat(diskNames).Distinct(StringComparer.OrdinalIgnoreCase);
            return Ok(merged);
        }
        [HttpGet("productFiles")]
        public async Task<IActionResult> GetProductFiles(int productId)
        {
            var all = await _repo.GetAllAsync();
            var productImages = all.Where(p => p.ProductId == productId);
            if (!productImages.Any())
                return NotFound("Ürüne ait dosya bulunamadı.");
            var names = productImages
                            .Select(p => Path.GetFileName(p.Url))
                            .Where(n => !string.IsNullOrWhiteSpace(n));
            return Ok(names);
        }
    }
}
