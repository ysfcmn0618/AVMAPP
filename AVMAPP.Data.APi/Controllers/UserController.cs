using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Services.Helpers;
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

        [HttpPost("login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LogInDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid user credentials.");
            }

            // Email'e göre kullanıcıyı Role bilgisi ile çek
            var existingUser = await repo.GetSingleWithIncludeAsync(
                u => u.Email == loginDto.Email,
                u => u.Role
            );

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Şifre doğrulama
            bool isPasswordValid = PasswordHelper.VerifyPassword(loginDto.Password, existingUser.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Burada JWT token oluşturabilirsin
            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    existingUser.Id,
                    existingUser.Email,
                    RoleName = existingUser.Role?.Name
                }
            });
        }


    }
}
