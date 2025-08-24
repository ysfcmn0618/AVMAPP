
namespace AVMAPP.Models.DTo.Dtos
  
{
    public class CartItemDto
    {       
            public Guid UserId { get; set; }
            public int ProductId { get; set; }
            public byte Quantity { get; set; }          
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }       
    }
}
