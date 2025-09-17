using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Data.Entities;
using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Models.DTo.Models.Auth;
using AVMAPP.Models.DTo.Models.Home;
using AVMAPP.Models.DTo.Models.Product;
using AVMAPP.Models.DTO.Dtos;
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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap();
            CreateMap<RegisterUserViewModel, UserDto>()
           .ReverseMap();
           
        }
    }
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, RoleDto>()
                .ReverseMap();
        }
    }
    public class  FormMessageProfile:Profile
    {
        public FormMessageProfile()
        {
            CreateMap<ContactFormEntity, NewContactFormMessageViewModel>()
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
            // DTO → Entity mapping
            CreateMap<ProductCommentDto, ProductCommentEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Entity → DTO mapping (isteğe bağlı)
            CreateMap<ProductCommentEntity, ProductCommentDto>();
        }
    }
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Entity -> DTO
            CreateMap<ProductEntity, ProductDto>()
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images))
            .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category))
            .ForMember(d => d.DiscountDto, opt => opt.MapFrom(s => s.Discount))
            .ForMember(d => d.Comments, opt => opt.MapFrom(s => s.Comments))
            .ReverseMap();

            CreateMap<ProductDto, ProductListingViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Undefined"))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountDto != null ? src.DiscountDto.DiscountRate : (byte?)null))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images != null && src.Images.Any() ? src.Images.First().Url : null))
            .ForMember(dest => dest.DiscountedPrice,opt => opt.MapFrom(src => src.DiscountDto != null ? src.Price - (src.Price * src.DiscountDto.DiscountRate / 100) : (decimal?)null)); ;

            // DTO -> SaveProductViewModel
            CreateMap<ProductDto, SaveProductViewModel>()
                .ReverseMap();
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
