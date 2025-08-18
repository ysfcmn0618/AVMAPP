using AutoMapper;
using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Color { get; set; } = "#FFFFFF"; // Varsayılan beyaz renk
        public string Icon { get; set; } = "icon-avg";     

    }

    public class  CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color ?? "#FFFFFF"))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon ?? "icon-avg")).ReverseMap();
        }
    }    
}
