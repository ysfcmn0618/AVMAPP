using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class ProductEntity : IGenericField
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public UserEntity Seller { get; set; } = null!;
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;
        public int? DiscountId { get; set; }
        public DiscountEntity? Discount { get; set; }       
        public string Name { get; set; } = "Undefined";
        public decimal Price { get; set; }
        public string Description { get; set; } = "Undefined";
        public int StockAmount { get; set; }=0;
        [JsonIgnore]
        public virtual ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();
        public virtual ICollection<ProductCommentEntity> Comments { get; set; } = new List<ProductCommentEntity>();

        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
