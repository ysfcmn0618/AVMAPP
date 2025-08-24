using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Data.Entities;
using AVMAPP.Models.DTo.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AVMAPP.Services.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemEntity, OrderItemDto>()
                .ReverseMap();
        }
    }
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, OrderEntity>().ReverseMap();
        }


    }
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color ?? "#FFFFFF"))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon ?? "icon-avg")).ReverseMap();
        }
    }
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItemEntity, CartItemDto>()
                .ReverseMap();
        }
    }
    public class ContactFormProfile : Profile
    {
        public ContactFormProfile()
        {
            CreateMap<ContactFormDto, ContactFormEntity>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember, ctx) => dest.Id == 0);
                    opt.MapFrom(_ => DateTime.UtcNow);
                })
                .ForMember(dest => dest.IsActive, opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember, ctx) => dest.Id == 0);
                    opt.MapFrom(_ => true);
                })
                .ForMember(dest => dest.IsDeleted, opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember, ctx) => dest.Id == 0);
                    opt.MapFrom(_ => false);
                });
            CreateMap<ContactFormEntity, ContactFormDto>();
        }
    }
    public class ProductCommentProfile : Profile
    {
        public ProductCommentProfile()
        {
            CreateMap<ProductCommentEntity, ProductCommentDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsConfirmed, opt => opt.Ignore())
            ;
        }
    }
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductDto>().ReverseMap();
        }
    }
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImageEntity, ProductImageDto>()
                .ReverseMap()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }

    }
}
