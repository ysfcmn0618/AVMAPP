

namespace AVMAPP.Data.APi.Models.Dtos
{
    public class ContactFormDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public bool IsActive { get; set; }=true;
        public bool IsDeleted { get; set; } = false;
    }
   
}
