using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Product
{
    public class SaveProductViewModel
    {
        public Guid SellerId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? DiscountId { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required, Range(typeof(decimal), "0.01", "79228162514264337593543950335"), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required, Range(1, 255)]
        public byte StockAmount { get; set; }
        public IList<IFormFile> Images { get; set; } = [];
    }
}
