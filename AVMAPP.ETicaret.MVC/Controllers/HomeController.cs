using AutoMapper;
using AVMAPP.Data.Entities;
using AVMAPP.ETicaret.MVC.Models;
using AVMAPP.Models.DTo.Models.Home;
using AVMAPP.Models.DTo.Models.Product;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    public class HomeController(IMapper mapper,IHttpClientFactory httpClient) : BaseController
    {      
        private HttpClient Client => httpClient.CreateClient("ApiClient");
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }
        [HttpGet("/contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost("/contact")]
        public async Task<IActionResult> Contact([FromForm] NewContactFormMessageViewModel newContactMessage)
        {
            if (!ModelState.IsValid)
            {
                return View(newContactMessage);
            }

            var contactMessageEntity = mapper.Map<ContactFormEntity>(newContactMessage);

            var response = await Client.PostAsJsonAsync("/contact-form", contactMessageEntity);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Mesajınız gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return View(newContactMessage);
            }

            SetSuccessMessage("Mesajınız başarı ile iletildi");

            return View();
        }
        [HttpGet("/products")]
        public async Task<IActionResult> Products()
        {
            try
            {
                var products = await Client.GetFromJsonAsync<IEnumerable<ProductListingViewModel>>("/Products");

                if (products == null || !products.Any())
                {
                    ViewBag.Message = "Ürün bulunamadı.";
                    return View(new List<ProductListingViewModel>());
                }

                return View(products);
            }
            catch (HttpRequestException ex)
            {
                // API erişim hatası
                ViewBag.Message = "API’ye ulaşılamıyor: " + ex.Message;
                return View(new List<ProductListingViewModel>());
            }
        }

        [HttpGet("/product/{productId:int}/details")]
        public async Task<IActionResult> ProductDetail([FromRoute] int productId)
        {

            var response = await Client.GetAsync($"/products/{productId}/home");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<HomeProductDetailViewModel>();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
