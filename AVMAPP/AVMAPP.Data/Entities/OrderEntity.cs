using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class OrderEntity: IGenericField
    {
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        //public string Address { get; set; }
        public string OrderCode { get; set; }
        public ICollection<OrderItemEntity>? OrderItems { get; set; } 
        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Id { get; set; }
    }
}
