using AVMAPP.Models.DTo.Models.Product;

namespace AVMAPP.Models.DTo.Models.ViewModels
{
    public class OwlCarouselViewModel
    {
        public string Title { get; set; } = null!;

        public List<ProductListingViewModel> Items { get; set; } = null!;
    }
}