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
    public class CartItemController(IGenericRepository<CartItemEntity> repo,IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await repo.GetAllAsync();
            if (cartItems is null || !cartItems.Any())
            {
                return NotFound("No cart items found.");
            }
            var cartItemDtos = mapper.Map<IEnumerable<CartItemDto>>(cartItems);
            return Ok(cartItemDtos);
        }
        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedCartItems()
        {
            var deletedCartItems = await repo.GetAllIncludingAsync(x => x.IsDeleted);
            if (deletedCartItems is null || !deletedCartItems.Any())
            {
                return NotFound("No deleted cart items found.");
            }
            var deletedCartItemDtos = mapper.Map<IEnumerable<CartItemDto>>(deletedCartItems);
            return Ok(deletedCartItemDtos);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCartItems()
        {            
            var activeCartItems = await repo.GetAllIncludingAsync(x => x.IsActive);
            if (activeCartItems is null || !activeCartItems.Any())
            {
                return NotFound("No active cart items found.");
            }
            var activeCartItemDtos = mapper.Map<IEnumerable<CartItemDto>>(activeCartItems);
            return Ok(activeCartItemDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            var cartItem = await repo.GetByIdAsync(id);
            if (cartItem is null)
            {
                return NotFound($"Cart item with ID {id} not found.");
            }
            var cartItemDto = mapper.Map<CartItemDto>(cartItem);
            return Ok(cartItemDto);
        }
        [HttpPost]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            if (cartItemDto is null)
            {
                return BadRequest("Cart item data is null.");
            }
            var cartItemEntity = mapper.Map<CartItemEntity>(cartItemDto);
            var addedCartItem = await repo.Add(cartItemEntity);
            var addedCartItemDto = mapper.Map<CartItemDto>(addedCartItem);
            return CreatedAtAction(nameof(GetCartItemById),  addedCartItemDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemDto cartItemDto)
        {
            if (cartItemDto is null)
            {
                return BadRequest("Cart item data is null.");
            }
            var existingCartItem = await repo.GetByIdAsync(id);
            if (existingCartItem is null)
            {
                return NotFound($"Cart item with ID {id} not found.");
            }
            var updatedCartItemEntity = mapper.Map<CartItemEntity>(cartItemDto);
            updatedCartItemEntity.UpdatedAt = DateTime.UtcNow;
            await repo.Update(updatedCartItemEntity);
            return Ok(updatedCartItemEntity);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var existingCartItem = await repo.GetByIdAsync(id);
            if (existingCartItem is null)
            {
                return NotFound($"Cart item with ID {id} not found.");
            }
            if(!existingCartItem.IsDeleted)
            {
                return BadRequest("Cart silindi olarak işaretlenmemiş.!");
            }
            if(existingCartItem.Quantity > 0)
            {
                return BadRequest("Mevcutta hala miktar görünüyor.");
            }
            await repo.Delete(id);
            return NoContent();
        }
}
