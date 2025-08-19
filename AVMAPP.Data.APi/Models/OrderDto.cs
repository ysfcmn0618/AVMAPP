using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }       
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // DTO → Entity
            CreateMap<OrderDto, OrderEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())                  // Id DB tarafından veya Controller'da set edilecek
                .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) // otomatik set
                .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) // otomatik set
                .ForMember(d => d.IsActive, opt => opt.MapFrom(_ => true))             // yeni order default aktif
                .ForMember(d => d.IsDeleted, opt => opt.MapFrom(_ => false))           // default silinmemiş
                .ForMember(d => d.User, opt => opt.Ignore())                           // navigation property maplenmez
                .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.UserId));      // sadece UserId üzerinden ilişki kur

            // Entity → DTO
            CreateMap<OrderEntity, OrderDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.User.FullName)); // örnek: User.Name'i DTO'ya yaz

        }


    }
}
 
