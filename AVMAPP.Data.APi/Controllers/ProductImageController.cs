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
    public class ProductImageController(IGenericRepository<ProductImageEntity> repo, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await repo.GetAllAsync();
            var dtos = mapper.Map<IEnumerable<ProductImageDto>>(entities);
            return Ok(dtos);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ProductImageDto>(entity);
            return Ok(dto);
        }
        [HttpGet("by-product/{productId:int}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var all = await repo.GetAllAsync();
            var filtered = all.Where(pi => pi.ProductId == productId);
            if (!filtered.Any())
            {
                return NotFound();
            }
            var dtos = mapper.Map<IEnumerable<ProductImageDto>>(filtered);
            return Ok(dtos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductImageDto dto)
        {
            var entity = mapper.Map<ProductImageEntity>(dto);
            var createdEntity = await repo.Add(entity);
            var createdDto = mapper.Map<ProductImageDto>(createdEntity);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductImageDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            var entity = mapper.Map<ProductImageEntity>(dto);
            var updatedEntity = await repo.Update(entity);
            if (updatedEntity is null)
            {
                return NotFound();
            }
            var updatedDto = mapper.Map<ProductImageDto>(updatedEntity);
            return Ok(updatedDto);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedEntity = await repo.Delete(id);
            if (deletedEntity == null)
            {
                return NotFound();
            }
            var deletedDto = mapper.Map<ProductImageDto>(deletedEntity);
            return Ok(deletedDto);
        }

    }
}
