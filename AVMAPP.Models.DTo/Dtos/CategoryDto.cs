
namespace AVMAPP.Data.APi.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Undefined";
        public string Color { get; set; } = "#FFFFFF"; // Varsayılan beyaz renk
        public string Icon { get; set; } = "icon-avg";     

    }      
}
