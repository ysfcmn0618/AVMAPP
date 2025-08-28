using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Product
{
    public class SaveProductCommentViewModel
    {
        [Required, MinLength(5), MaxLength(500)]
        public string Comment { get; set; } = null!;

        [Required, Range(1, 5)]
        public byte StarCount { get; set; }

        public Guid UserId { get; set; }
    }
}
