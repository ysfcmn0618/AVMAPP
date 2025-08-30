using AVMAPP.Admin.MVC.Models;
using AVMAPP.Data.APi.Models;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Models.DTO.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace AVMAPP.Admin.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");

        [Route("/products/")]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ProductFilterViewModel filter)
        {
            var model = new ProductListViewModel
            {
                Filter = filter
            };

            // JWT token ekle
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // API’den kategori listesi al
            var catResponse = await Client.GetAsync("api/category/");
            if (catResponse.IsSuccessStatusCode)
            {
                var catJson = await catResponse.Content.ReadAsStringAsync();
                model.Categories = JsonConvert.DeserializeObject<List<CategoryDto>>(catJson) ?? new();
            }

            // API’den ürünleri filtreli olarak al
            string queryString = BuildQueryString(filter);
            var response = await Client.GetAsync($"api/product/filter{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                model.Products = JsonConvert.DeserializeObject<List<ProductDto>>(jsonData) ?? new();
            }
            else
            {
                ViewBag.Error = "Ürünler alınamadı: " + response.StatusCode;
            }

            return View(model);
        }



        // GET: /products/{id}/edit
        [HttpGet]
        [Route("products/{id:int}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            // API'den ürün detayını çek
            var response = await Client.GetAsync($"api/products/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Ürün bulunamadı.";
            }
            else
            {
                TempData["Error"] = "Ürün yüklenirken bir hata oluştu.";
            }

            var json = await response.Content.ReadAsStringAsync();
            var product = System.Text.Json.JsonSerializer.Deserialize<ProductDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Kategori listesini API’den çek
            var categoryResponse = await Client.GetAsync("api/categories");
            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDto>>(categoryJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ViewBag.Categories = categories;

            return View(product);
        }

        // POST: /products/{id}/edit
        [HttpPost("{id:int}/edit")]
        public async Task<IActionResult> Edit(int id, ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                // Tekrar kategori listesini yükle
                var categoryResponse = await Client.GetAsync("api/categories");
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categories = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDto>>(categoryJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                ViewBag.Categories = categories;

                return View(model);
            }

            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
            );

            var response = await Client.PutAsync($"api/products/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Ürün güncellenirken bir hata oluştu.";
            return View(model);
        }



        // Delete metodu
        [Route("/products/{productId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await Client.DeleteAsync($"api/product/{productId}");

            if (!response.IsSuccessStatusCode)
                TempData["ErrorMessage"] = $"Ürün silinemedi: {response.StatusCode}";
            else
                TempData["SuccessMessage"] = "Ürün başarıyla silindi.";

            return RedirectToAction("List");
        }

        // Helper: ProductFilterViewModel’den query string oluştur
        private string BuildQueryString(ProductFilterViewModel filter)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(filter.Name)) query["Name"] = filter.Name;
            if (filter.CategoryId.HasValue) query["CategoryId"] = filter.CategoryId.Value.ToString();
            if (filter.MinPrice.HasValue) query["MinPrice"] = filter.MinPrice.Value.ToString();
            if (filter.MaxPrice.HasValue) query["MaxPrice"] = filter.MaxPrice.Value.ToString();
            if (filter.IsActive.HasValue) query["IsActive"] = filter.IsActive.Value.ToString();

            string qs = query.ToString();
            return string.IsNullOrEmpty(qs) ? "" : "?" + qs;
        }
    }
}
