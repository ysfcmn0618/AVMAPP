using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models.Dtos
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public string OrderCode { get; set; }
        public ICollection<OrderItemEntity>? OrderItems { get; set; }
        // IGenericField alanları
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
   
}
 
