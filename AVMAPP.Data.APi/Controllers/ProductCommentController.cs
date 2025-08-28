using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using AVMAPP.Models.DTo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AVMAPP.Data.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentController(IGenericRepository<ProductCommentEntity> repo, IMapper mapper) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await repo.GetAllAsync();
            var result = mapper.Map<IEnumerable<ProductCommentDto>>(comments);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await repo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var result = mapper.Map<ProductCommentDto>(comment);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCommentDto comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existingcomment = await repo.GetByIdAsync(id);
            if (existingcomment == null)
            {
                return BadRequest("Yorum bulunamadı");
            }

            mapper.Map(comment, existingcomment);
            comment.UpdatedAt = DateTime.UtcNow;
            await repo.Update(existingcomment);

            return Ok(mapper.Map<ProductCommentDto>(existingcomment));
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await repo.GetByIdAsync(id);
            if (comment == null) return NotFound("Yorum bulunamadı.");
            await repo.Delete(id);

            return NoContent();
        }
        [Authorize]
        [HttpPost("{productId:int}/comment")]
        public async Task<IActionResult> AddComment(int productId, [FromBody] ProductCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await repo.GetByIdAsync(productId);
            if (product is null)
            {
                return NotFound(new { message = "Ürün bulunamadı." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            // DTO → Entity
            var entity = mapper.Map<ProductCommentEntity>(commentDto);
            entity.ProductId = productId;
            entity.UserId = userId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            await repo.Add(entity);

            return Ok(new { message = "Yorum başarıyla eklendi." });
        }




    }
}
