using AVMAPP.Admin.MVC.Models;
using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Models.DTO.Dtos;
using AVMAPP.Models.DTO.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AVMAPP.Admin.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");

        // Kullanıcı listesi
        [Route("/users/")]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] UserFilterViewModel filter)
        {
            var model = new UserListViewModel
            {
                Filter = filter
            };

            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string queryString = BuildQueryString(filter);
            var response = await Client.GetAsync($"api/user{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                model.Users = JsonConvert.DeserializeObject<List<UserDto>>(jsonData) ?? new();
            }
            else
            {
                ViewBag.Error = "Kullanıcılar alınamadı: " + response.StatusCode;
            }

            return View(model);
        }

        // Kullanıcı silme
        [Route("/users/{userId:guid}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await Client.DeleteAsync($"api/user/{userId}");

            if (!response.IsSuccessStatusCode)
                TempData["ErrorMessage"] = $"Kullanıcı silinemedi: {response.StatusCode}";
            else
                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";

            return RedirectToAction("List");
        }

        // Kullanıcı düzenleme (GET)
        [Route("/users/{userId:guid}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid userId)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await Client.GetAsync($"api/user/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("List");
            }

            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("List");
            }
            // Rol listesini API’den çek
            var rolesResponse = await Client.GetAsync("api/role"); // API’deki tüm rolleri dönen endpoint
            List<RoleDto> roles = new List<RoleDto>();
            if (rolesResponse.IsSuccessStatusCode)
            {
                var rolesJson = await rolesResponse.Content.ReadAsStringAsync();
                roles = JsonConvert.DeserializeObject<List<RoleDto>>(rolesJson) ?? new();
            }
            ViewBag.Roles = roles;
            return View(user);
        }

        // Kullanıcı düzenleme (POST)
        [Route("/users/{userId:guid}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] Guid userId, UserDto editedUser)
        {
            if (!ModelState.IsValid)
                return View(editedUser);

            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                Client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await Client.PutAsJsonAsync($"api/user/{userId}", editedUser);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Güncelleme başarısız oldu.");
                return View(editedUser);
            }

            TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("List");
        }

        // Helper: query string
        private string BuildQueryString(UserFilterViewModel filter)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(filter.Name)) query["Name"] = filter.Name;
            if (!string.IsNullOrWhiteSpace(filter.Email)) query["Email"] = filter.Email;
            if (filter.RoleId.HasValue) query["RoleId"] = filter.RoleId.Value.ToString();
            if (filter.EmailConfirmed.HasValue) query["EmailConfirmed"] = filter.EmailConfirmed.Value.ToString();

            string qs = query.ToString();
            return string.IsNullOrEmpty(qs) ? "" : "?" + qs;
        }
    }
}
