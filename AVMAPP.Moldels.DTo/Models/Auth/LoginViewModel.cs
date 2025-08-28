using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.EmailAddress), MaxLength(256)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.Password)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Password { get; set; } = null!;
    }
}
