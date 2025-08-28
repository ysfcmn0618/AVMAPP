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
        [HttpGet("/product/list")]
        public async Task<IActionResult> Listing()
        {
            // TODO: add paging support

            var response = await Client.GetAsync("/products");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductListingViewModel>>();

            return View(products);
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
