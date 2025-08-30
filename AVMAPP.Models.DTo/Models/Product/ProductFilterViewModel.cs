using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Models.Product
{
    public class ProductFilterViewModel
    {
        public string? Name { get; set; }        
        public int? CategoryId { get; set; }     
        public decimal? MinPrice { get; set; }   
        public decimal? MaxPrice { get; set; }   
        public bool? IsActive { get; set; }      
    }

}
