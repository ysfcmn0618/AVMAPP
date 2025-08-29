using AVMAPP.Models.DTo.Models.Cart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Order
{
    public class CheckoutViewModel
    {
        [Required, MinLength(10), MaxLength(250), DataType(DataType.MultilineText)]
        public string Address { get; set; } = null!;
        public IEnumerable<CartItemViewModel> CartItems { get; set; }
    }
}
