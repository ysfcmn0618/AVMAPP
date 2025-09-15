using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.Data.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IGenericRepository<OrderEntity> repo, IMapper mapper) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await repo.GetAllAsync();
            if (orders is null || !orders.Any())
            {
                return NotFound("No orders found.");
            }
            var orderDtos = mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Kullanıcının böyle bir siparişi var mı?
            var order = await repo.GetByIdAsync(id);
            if (order is null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            var orderDto = mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }
        [Authorize]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var orders = await repo.GetAllIncludingAsync(o => o.UserId == userId);
            if (orders is null || !orders.Any())
            {
                return NotFound($"No orders found for user with ID {userId}.");
            }
            var orderDtos = mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto orderDto)
        {
            if (orderDto is null)
            {
                return BadRequest("Order data is null.");
            }
            var orderEntity = mapper.Map<OrderEntity>(orderDto);
            var createdOrder = await repo.Add(orderEntity);
            var createdOrderDto = mapper.Map<OrderDto>(createdOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrderDto);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderDto)
        {
            if (orderDto is null)
            {
                return BadRequest("Order data is null.");
            }
            var existingOrder = await repo.GetByIdAsync(id);
            if (existingOrder is null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            var orderEntity = mapper.Map<OrderEntity>(orderDto);
            orderEntity.Id = id; // Ensure the ID is set for the update
            orderEntity.UpdatedAt = DateTime.UtcNow; // Update the timestamp
            var updatedOrder = await repo.Update(orderEntity);
            var updatedOrderDto = mapper.Map<OrderDto>(updatedOrder);
            return Ok(updatedOrderDto);
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingOrder = await repo.GetByIdAsync(id);
            if (existingOrder is null)
            {
                return NotFound($"Order with ID {id} not found.");
            }
            await repo.Delete(id);
            return NoContent(); // 204 No Content
        }
    }
}
