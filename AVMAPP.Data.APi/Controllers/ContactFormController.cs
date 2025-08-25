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
    public class ContactFormController(IGenericRepository<ContactFormEntity> repo, IMapper mapper) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contactForms = await repo.GetAllAsync();
            var contactFormDtos = mapper.Map<IEnumerable<ContactFormDto>>(contactForms);
            return Ok(contactFormDtos);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contactForm = await repo.GetByIdAsync(id);
            if (contactForm == null)
            {
                return NotFound();
            }
            var contactFormDto = mapper.Map<ContactFormDto>(contactForm);
            contactFormDto.SeenAt = DateTime.UtcNow;
            return Ok(contactFormDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactFormDto contactFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (contactFormDto == null)
            {
                return BadRequest("Contact form data is null.");
            }
            var contactFormEntity = mapper.Map<ContactFormEntity>(contactFormDto);
            await repo.Add(contactFormEntity);
            return CreatedAtAction(nameof(GetById), new { id = contactFormEntity.Id }, contactFormDto);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContactFormDto contactFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (contactFormDto == null)
            {
                return BadRequest("Contact form data is null.");
            }
            var existingContactForm = await repo.GetByIdAsync(id);
            if (existingContactForm == null)
            {
                return NotFound();
            }
            var contactFormEntity = mapper.Map<ContactFormEntity>(contactFormDto);
            contactFormEntity.UpdatedAt = DateTime.UtcNow;
            await repo.Update(contactFormEntity);
            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingContactForm = await repo.GetByIdAsync(id);
            if (existingContactForm == null)
            {
                return NotFound();
            }
            if (!existingContactForm.IsDeleted)
            {
                return BadRequest("Form silindi olarak işaretlenmemiş");
            }
            await repo.Delete(id);
            return NoContent();
        }
    }
}
