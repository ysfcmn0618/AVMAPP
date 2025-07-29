using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class CartItemEntity:IGenericField
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public byte Quantity { get; set; }

        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
