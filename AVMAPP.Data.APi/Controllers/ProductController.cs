using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using AVMAPP.Models.DTO.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AVMAPP.Data.APi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IGenericRepository<ProductEntity> repo, IMapper mapper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await repo.GetAllAsync();
            if (products == null) return NotFound("Ürün Bulunamadı.");
            return Ok(mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound("Ürün Bulunamadı.");
            return Ok(mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> Add([FromBody] ProductDto productDto)
        {
            if (productDto == null) return BadRequest("Ürün bilgileri eksik.");
            var product = mapper.Map<ProductEntity>(productDto);
            product.CreatedAt = DateTime.UtcNow;            
            var addedProduct = await repo.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, mapper.Map<ProductDto>(addedProduct));
        }

        [Authorize(Policy = "AdminOrSeller")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null || id != productDto.Id) return BadRequest("Ürün bilgileri eksik veya ID uyuşmuyor.");
            var existingProduct = await repo.GetByIdAsync(id);
            if (existingProduct == null) return NotFound("Ürün Bulunamadı.");
            var updatedProduct = mapper.Map(productDto, existingProduct);
            updatedProduct.UpdatedAt = DateTime.UtcNow; // Güncelleme tarihini ayarla
            await repo.Update(updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOrSeller")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound("Ürün Bulunamadı.");
            await repo.Delete(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] ProductFilterViewModel filter)
        {
            var query = repo.Query().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(p => p.IsActive == filter.IsActive.Value);

            var products = await query.ToListAsync();

            return Ok(mapper.Map<List<ProductDto>>(products));
        }

    }
}
