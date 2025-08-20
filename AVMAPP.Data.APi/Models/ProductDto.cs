using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class ProductDto
    {
        public Guid SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public byte StockAmount { get; set; } = 0;
        public virtual ICollection<ProductImageEntity> Images { get; set; } = null!;
        public virtual ICollection<ProductCommentEntity> Comments { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
    public class ProductProfile : Profile
    {
        public ProductProfile() {
            CreateMap<ProductEntity, ProductDto>().ReverseMap();
        }
    }
}
