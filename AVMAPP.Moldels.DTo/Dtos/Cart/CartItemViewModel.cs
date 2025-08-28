using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos.Cart
{
    public class CartItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;

        public string? ProductImage { get; set; }

        public byte Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => Quantity * Price;
    }
}
