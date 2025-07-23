using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class ProductEntity : IGenericField
    {
        public string SellerId { get; set; } = null!;
        public UserEntity Seller { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public byte StockAmount { get; set; }=0;
        public virtual ICollection<ProductImageEntity> Images { get; set; } = null!;
        public virtual ICollection<ProductCommentEntity> ProductComments { get; set; } = null!;

        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Id { get; set; }
    }
}
