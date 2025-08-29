using System.ComponentModel.DataAnnotations;

namespace AVMAPP.Models.DTo.Models.ViewModels

{
    public class ProfileDetailsViewModel
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = null!;

        public string? Password { get; set; }
    }
}