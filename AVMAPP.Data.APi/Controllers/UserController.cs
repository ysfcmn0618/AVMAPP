using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.Data.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IGenericRepository<UserEntity> repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await repo.GetAllAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await repo.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("login", Name = "GetUser")]
        public async Task<IActionResult> Login([FromBody] UserEntity user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user credentials.");
            }
            var existingUser = await repo.GetSingleWithIncludeAsync(
                 u => u.Email == user.Email && u.Password == user.Password,
                 u => u.Role
);
            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(existingUser);
        }


    }
}
