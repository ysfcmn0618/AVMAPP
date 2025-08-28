using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    public class ProductCommentEntity:GenericField
    {
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
        public Guid UserId { get; set; }
        public UserEntity? User { get; set; }
        public string? Comment { get; set; }
        public byte StarCount { get; set; }
        public bool IsConfirmed { get; set; }       
    }
}
