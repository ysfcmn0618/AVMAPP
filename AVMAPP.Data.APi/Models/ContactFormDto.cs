using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class ContactFormDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public bool IsActive { get; set; }=true;
        public bool IsDeleted { get; set; } = false;
    }
    public class ContactFormProfile:Profile
    {
        public ContactFormProfile()
        {
            CreateMap<ContactFormDto, ContactFormEntity>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
            CreateMap<ContactFormEntity, ContactFormDto>();
        }
    }
}
