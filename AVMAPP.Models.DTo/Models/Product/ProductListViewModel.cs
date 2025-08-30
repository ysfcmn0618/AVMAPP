using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Models.Product
{
    public class ProductListViewModel
    {
        public List<ProductDto> Products { get; set; } = new();
        public ProductFilterViewModel Filter { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
    }

}
