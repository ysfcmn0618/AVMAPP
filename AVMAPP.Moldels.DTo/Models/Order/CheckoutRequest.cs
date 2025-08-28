using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Order
{
    public class CheckoutRequest
    {
        public Guid UserId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
