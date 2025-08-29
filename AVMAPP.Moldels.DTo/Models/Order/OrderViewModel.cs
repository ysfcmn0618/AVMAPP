using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Order
{
    public class OrderViewModel
    {
        public string OrderCode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalProducts { get; set; }
        public int TotalQuantity { get; set; }
    }
}
