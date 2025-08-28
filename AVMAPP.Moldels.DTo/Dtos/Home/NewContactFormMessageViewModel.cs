using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Dtos.Home
{
    public class NewContactFormMessageViewModel
    {

        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required, MinLength(10), MaxLength(1000)]
        public string Message { get; set; } = null!;
    }
}
