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
    public class UserController(IGenericRepository<UserEntity> repo,IConfiguration configuration) : ControllerBase
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
                return BadRequest("Invalid user credentials.");

            var existingUser = await repo.GetSingleWithIncludeAsync(
                u => u.Email == loginDto.Email,
                u => u.Role
            );

            if (existingUser == null)
                return Unauthorized("Invalid username or password.");

            if (!PasswordHelper.VerifyPassword(loginDto.Password, existingUser.Password))
                return Unauthorized("Invalid username or password.");

            // JWT üretimi
            var token = JwtHelper.GenerateToken(
                existingUser,
                secretKey: configuration["JwtSettings:SecretKey"],
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expirationMinutes: int.Parse(configuration["JwtSettings:ExpirationMinutes"])
            );

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
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
