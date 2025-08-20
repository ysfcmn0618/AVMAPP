using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class ProductCommentDto
    {
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public byte StarCount { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
    public class ProductCommentProfile:Profile
    {
        public ProductCommentProfile()
        {
            CreateMap<ProductCommentEntity, ProductCommentDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsConfirmed, opt => opt.Ignore())
            ;
        }
    }
}
