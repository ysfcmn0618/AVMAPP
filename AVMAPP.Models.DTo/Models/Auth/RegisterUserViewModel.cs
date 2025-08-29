using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Auth
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), MinLength(2, ErrorMessage = "En az 2 karakter girilmelidir"), MaxLength(50, ErrorMessage = "50 karakterden fazla giremezsiniz")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), MinLength(2, ErrorMessage = "En az 2 karakter girilmelidir"), MaxLength(50, ErrorMessage = "50 karakterden fazla giremezsiniz")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Surname { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.Password), MinLength(4, ErrorMessage = "En az 4 karakter girilmelidir.")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Boşluk ile başlamaz!")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Bu alan boş bırakılamaz!"), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Şifreler Eşleşmelidir.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
