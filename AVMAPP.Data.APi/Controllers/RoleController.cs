using AutoMapper;
using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure;
using AVMAPP.Models.DTO.Dtos;
using AVMAPP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AVMAPP.Data.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IGenericRepository<RoleEntity> _repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _repo.GetAll()
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return Ok(roles);
        }
    }
}
