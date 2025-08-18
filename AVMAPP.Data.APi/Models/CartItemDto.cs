using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class CartItemDto
    {       
            public Guid UserId { get; set; }
            public int ProductId { get; set; }
            public byte Quantity { get; set; }          
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }       
    }
    public class CartItemProfile:Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItemEntity, CartItemDto>()
                .ReverseMap();
        }
    }
}
