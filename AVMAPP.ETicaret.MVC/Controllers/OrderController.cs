using AutoMapper;
using AVMAPP.Models.DTo.Models.Cart;
using AVMAPP.Models.DTo.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class OrderController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");
        [HttpPost("/order")]
        public async Task<IActionResult> Create([FromForm] CheckoutViewModel model)
        {
            var userId = GetUserId();

            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await GetCartItemsAsync();
                return View(viewModel);
            }

            var response = await Client.PostAsJsonAsync("cart/checkout", new CheckoutRequest
            {
                UserId = userId.Value,
                Address = model.Address,
            });

            if (!response.IsSuccessStatusCode)
            {
                return CheckoutFailedResult(model, response);
            }

            var order = await response.Content.ReadFromJsonAsync<OrderDetailsViewModel>();

            if (order is null)
            {
                return CheckoutFailedResult(model, response);
            }

            return RedirectToAction(nameof(Details), new { orderCode = order.OrderCode });
        }
        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var cartItems = await GetCartItemsAsync();

            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                Address = ""// Boş başlatılabilir
            };

            return View(model); // Views/Order/Checkout.cshtml
        }
        private IActionResult CheckoutFailedResult(CheckoutViewModel model, HttpResponseMessage response)
        {
            // API'den gelen hata mesajını al
            var errorMessage = $"Ödeme işlemi başarısız oldu. Sunucu yanıtı: {(int)response.StatusCode} {response.ReasonPhrase}";

            // ModelState'e hata ekle
            ModelState.AddModelError(string.Empty, errorMessage);

            // Mevcut model ile tekrar checkout sayfasını göster
            return View("Checkout", model);
        }
        [HttpGet("/order/{orderCode}/details")]
        public async Task<IActionResult> Details([FromRoute] string orderCode)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var order = await Client.GetFromJsonAsync<OrderDetailsViewModel>($"order/{userId}/{orderCode}");

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
        private async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userId = GetUserId() ?? Guid.Empty;

            var response = await Client.GetAsync($"cart/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var cartItems = await response.Content.ReadFromJsonAsync<List<CartItemViewModel>>();

            return cartItems ?? [];
        }
    }
}
