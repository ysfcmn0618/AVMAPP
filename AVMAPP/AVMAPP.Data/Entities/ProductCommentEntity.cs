using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class ProductCommentEntity:IGenericField
    {
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public byte StarCount { get; set; }
        public bool IsConfirmed { get; set; }

        // IGenericField alanları
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Id { get; set; }
    }
}
