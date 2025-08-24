using AVMAPP.Data.Entities;

namespace AVMAPP.Data.APi.Models
{
    public class ProductImageDto
    {
        public int ProductId { get; set; }
        public ProductEntity? Product { get; set; }
        public string Url { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
   
}
