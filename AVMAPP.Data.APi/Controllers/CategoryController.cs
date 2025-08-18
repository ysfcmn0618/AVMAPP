using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.Data.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IGenericRepository<CategoryEntity> repo,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await repo.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await repo.GetByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Bu id ye:{id} sahip bir kategori bulunamadı.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgisi eksik.");
            }
            var addedCategory = await repo.Add(mapper.Map<CategoryEntity>(category));
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto category)
        {
           
            try
            {
                var existing = await repo.GetByIdAsync(id);
               
               existing= mapper.Map(category, existing);
                var updatedCategory = await repo.Update(existing);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Bu id ye:{id} sahip bir kategori bulunamadı.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedCategory = await repo.Delete(id);
                return Ok(deletedCategory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Bu id ye:{id} sahip bir kategori bulunamadı.");
            }
        }
    }
}
