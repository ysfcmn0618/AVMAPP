using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;


namespace AVMAPP.Models.DTO.Models.Product
{
    public class ProductListViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public ProductFilterViewModel Filter { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
    }

}
