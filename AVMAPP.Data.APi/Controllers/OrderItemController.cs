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
    public class OrderItemController(IGenericRepository<OrderItemEntity> repo,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await repo.GetAllIncludingAsync(u=>u.OrderId);
            return Ok(mapper.Map<IEnumerable<OrderItemDto>>(orderItems));
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] OrderItemDto orderItemDto)
        {
            if (id != orderItemDto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var orderItemEntity = mapper.Map<OrderItemEntity>(orderItemDto);
            var updatedOrderItem = await repo.Update(orderItemEntity);
            return Ok(mapper.Map<OrderItemDto>(updatedOrderItem));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderItemDto orderItemDto)
        {
            var orderItemEntity = mapper.Map<OrderItemEntity>(orderItemDto);
            var createdOrderItem = await repo.Add(orderItemEntity);
            return CreatedAtAction(nameof(GetAll), new { id = createdOrderItem.Id }, mapper.Map<OrderItemDto>(createdOrderItem));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await repo.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            await repo.Delete(id);
            return NoContent();
        }

    }
}
