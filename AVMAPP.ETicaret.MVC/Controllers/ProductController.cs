using AutoMapper;
using AVMAPP.Data.APi.Models;
using AVMAPP.Models.DTo.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    [Route("/product")]
    public class ProductController(IHttpClientFactory clientFactory,IMapper mapper) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");
        [HttpGet("")]
        [Authorize(Roles = "seller")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Create([FromForm] SaveProductViewModel newProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newProductModel);
            }

            var response = await Client.PostAsJsonAsync("/product", newProductModel);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the product. Please try again later.";
                return View(newProductModel);
            }

            SetSuccessMessage("Ürün başarıyla eklendi.");
            ModelState.Clear();

            return View();
        }
        [HttpGet("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var productDto = await response.Content.ReadFromJsonAsync<ProductDto>();

            if (productDto is null)
            {
                return NotFound();
            }

            if (productDto.SellerId != GetUserId())
            {
                return Forbid();
            }
            var viewModel = mapper.Map<SaveProductViewModel>(productDto);           

            return View(viewModel);
        }
        [HttpPost("{productId:int}/edit")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Edit([FromRoute] int productId, [FromForm] SaveProductViewModel editProductModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editProductModel);
            }

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var productDto = await response.Content.ReadFromJsonAsync<ProductDto>();

            if (productDto is null)
            {
                return NotFound();
            }

            if (GetUserId() is Guid currentUserId && productDto.SellerId != currentUserId)
            {
                return Forbid();
            }
            var product= mapper.Map<ProductDto>(editProductModel);
            product.Id = productId;

            response = await Client.PutAsJsonAsync($"/product/{productId}", product);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the product. Please try again later.";
                return View(editProductModel);
            }

            ViewBag.SuccessMessage = "Ürün başarıyla güncellendi.";
            return RedirectToAction("Details", new { productId });
        }
        [HttpPost("{productId:int}/delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {     
            var userId = GetUserId();

            if (userId is null)
            {
                return Unauthorized();
            }

            var response = await Client.GetAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var productEntity = await response.Content.ReadFromJsonAsync<ProductDto>();

            if (productEntity is null)
            {
                return NotFound();
            }

            if (productEntity.SellerId != userId)
            {
                return Forbid();
            }

            response = await Client.DeleteAsync($"/product/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the product. Please try again later.";
                return View();
            }

            SetSuccessMessage("Ürün başarıyla silindi.");
            return RedirectToAction("Index");//Liste sayfasına ayarlanacak
        }
        [HttpPost("{productId:int}/comment")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "buyer, seller")]
        public async Task<IActionResult> Comment(int productId, SaveProductCommentViewModel newComment)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Yorum eklenirken doğrulama hatası oluştu.";
                return RedirectToAction("Details", new { productId });
            }

            // ViewModel → DTO
            var commentDto = mapper.Map<ProductCommentDto>(newComment);

            // API çağrısı
            var response = await Client.PostAsJsonAsync($"/product/{productId}/comment", commentDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Yorum eklenirken bir hata oluştu.";
                return RedirectToAction("Details", new { productId });
            }

            TempData["SuccessMessage"] = "Yorum başarıyla eklendi.";
            return RedirectToAction("Details", new { productId });
        }

    }
}
