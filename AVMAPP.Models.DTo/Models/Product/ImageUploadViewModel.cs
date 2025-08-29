using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Models.DTo.Models.Product
{
    public class ImageUploadViewModel
    {
        public bool IsRequired { get; set; }
        public bool IsMultipleFiles { get; set; }
        public string Name { get; set; } = null!;
        public string? Label { get; set; }
        public string? Accept { get; set; }
    }
}
