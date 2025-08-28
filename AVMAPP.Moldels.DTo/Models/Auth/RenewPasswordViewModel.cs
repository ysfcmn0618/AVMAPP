using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Auth
{
    public class RenewPasswordViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Email { get; set; } = null!;
        [Required, MinLength(1)]
        public string Token { get; set; } = default!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.Password)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Şifreler Eşleşmelidir.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
