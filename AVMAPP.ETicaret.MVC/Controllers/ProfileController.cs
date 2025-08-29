using AVMAPP.Models.DTo.Models.Order;
using AVMAPP.Models.DTo.Models.Product;
using AVMAPP.Models.DTo.Models.Profile;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    [Authorize(Roles = "seller, buyer")]
    public class ProfileController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");
        [HttpGet("/profile")]
        public async Task<IActionResult> Details()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/api/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userViewModel = await response.Content.ReadFromJsonAsync<ProfileDetailsViewModel>();

            if (userViewModel is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? previousSuccessMessage = TempData["SuccessMessage"] as string;

            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }

            return View(userViewModel);
        }
        [HttpPost("/profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProfileDetailsViewModel editMyProfileModel)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await GetCurrentUserAsync();

            if (user is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }

            var payload = new
            {
                FirstName = editMyProfileModel.FirstName?.Trim(),
                LastName = editMyProfileModel.LastName?.Trim(),
                Password = string.IsNullOrWhiteSpace(editMyProfileModel.Password) || editMyProfileModel.Password == "******"
                            ? null
                            : editMyProfileModel.Password
            }
            ;
            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }
            ;
            var response = await Client.PutAsJsonAsync($"/api/user/{user.Id}", payload, jsonOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = $"Bir hata oluştu (Status: {(int)response.StatusCode}).";
                if (!string.IsNullOrWhiteSpace(errorContent))
                {
                    errorMessage += $" Detay: {errorContent}";
                }
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";

            return RedirectToAction(nameof(Details));
        }
        [HttpGet("/my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/api/order/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();

            return View(orders ?? []);
        }
        [HttpGet("/my-products")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> MyProducts()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/api/products?sellerId={userId}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var products = await response.Content.ReadFromJsonAsync<List<MyProductsViewModel>>();

            return View(products ?? []);
        }
    }
}
