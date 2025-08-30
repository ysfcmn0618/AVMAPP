using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Data.Entities;
using AVMAPP.Models.DTo.Dtos;

namespace AVMAPP.Data.APi.Models
{
    public class ProductDto
    {
        public Guid SellerId { get; set; }
        public int CategoryId { get; set; }
        public int? DiscountId { get; set; }
        public DiscountDto? DiscountDto { get; set; }
        public string Name { get; set; } = "Undefined";
        public decimal Price { get; set; }
        public string Description { get; set; } = "Undefined";
        public byte StockAmount { get; set; } = 0;
        public CategoryDto? Category { get; set; }
        public ICollection<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public ICollection<ProductCommentDto> Comments { get; set; } = new List<ProductCommentDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
   
}
