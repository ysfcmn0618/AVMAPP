using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class CategoryEntity 
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        // Her kategoriye ait birden fazla ürün olabilir
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();


        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
