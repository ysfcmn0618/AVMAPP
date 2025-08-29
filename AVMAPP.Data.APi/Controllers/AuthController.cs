using AutoMapper;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Services;
using AVMAPP.Services.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AVMAPP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IGenericRepository<UserEntity> _repo, TokenService _tokenService,IMapper mapper) : ControllerBase
    {
        

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Email and password are required.");

            var existingUser = await _repo.Query()
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (existingUser == null)
                return Unauthorized("Invalid email or password.");

            if (!PasswordHelper.VerifyPassword(loginDto.Password, existingUser.Password))
                return Unauthorized("Invalid email or password.");

            var token = _tokenService.GenerateToken(existingUser.Id, existingUser.Email, existingUser.Role.Name);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    existingUser.Id,
                    existingUser.Email,
                    Roles = existingUser.Role.Name
                }
            });
        }
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { Message = "Logout successful. Please remove the token on client side." });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            var user = mapper.Map<UserEntity>(dto);
            await _repo.Add(user);
            return Ok(new { message = "Kayıt başarılı" });
        }
    }
}
