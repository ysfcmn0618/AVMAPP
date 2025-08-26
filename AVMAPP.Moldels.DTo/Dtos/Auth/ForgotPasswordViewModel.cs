using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos.Auth
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Email { get; set; } = null!;
    }
}
