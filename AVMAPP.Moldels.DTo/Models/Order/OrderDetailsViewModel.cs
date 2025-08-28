using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Order
{
    public class OrderDetailsViewModel
    {
        public string OrderCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = [];
    }
}
