using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class CategoryEntity 
    {
        public string Name { get; set; }
        public string Color { get; set; }= "#FF5733"; // Varsayılan beyaz renk
        public string Icon { get; set; } = "icon-avg";
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
