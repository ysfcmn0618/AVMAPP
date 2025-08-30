using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTO.Models.Category
{
    public class SaveCategoryViewModel
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;

        public string? Icon { get; set; } = string.Empty;
    }
}
