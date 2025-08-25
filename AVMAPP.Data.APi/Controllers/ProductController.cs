using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.Data.APi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IGenericRepository<ProductEntity> repo, IMapper mapper) : ControllerBase
    {
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
        public async Task<IActionResult> Add([FromBody] ProductDto productDto)
        {
            if (productDto == null) return BadRequest("Ürün bilgileri eksik.");
            var product = mapper.Map<ProductEntity>(productDto);
            product.CreatedAt = DateTime.UtcNow;            
            var addedProduct = await repo.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, mapper.Map<ProductDto>(addedProduct));
        }
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
        public async Task<IActionResult> Delete(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound("Ürün Bulunamadı.");
            await repo.Delete(id);
            return NoContent();
        }
    }
}
