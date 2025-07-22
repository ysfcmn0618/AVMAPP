using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class ProductImageEntity : IGenericField
    {
        public string ProductId { get; set; } = null!;
        public string Url { get; set; } = null!;

        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Id { get; set; }
    }
}
