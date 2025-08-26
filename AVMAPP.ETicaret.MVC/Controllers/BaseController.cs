using AVMAPP.Models.DTo.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    public abstract class BaseController : Controller
    {
        protected T? GetService<T>() where T : class => HttpContext.RequestServices.GetService<T>();

        protected void SetSuccessMessage(string message) => ViewBag.SuccessMessage = message;

        protected void SetErrorMessage(string message) => ViewBag.ErrorMessage = message;

        protected string? GetCookie(string key) => Request.Cookies[key];

        protected void SetCookie(string key, string value) => Response.Cookies.Append(key, value);

        protected void RemoveCookie(string key) => Response.Cookies.Delete(key);

        protected async Task<UserDto?> GetCurrentUserAsync()
        {

            var userId = GetUserId();
            if (userId is null) return null;

            var clientFactory = GetService<IHttpClientFactory>();
            if (clientFactory == null) return null;

            var client = clientFactory.CreateClient("Api.Data");

            var response = await client.GetAsync($"api/user/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            return user;
        }

        protected bool IsUserLoggedIn() => /*GetCookie("userId") != null;*/User.Identity?.IsAuthenticated ?? false;

        protected int? GetUserId() => /*int.TryParse(GetCookie("userId"), out int userId) ? userId : null;*/int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null;


        protected async Task<bool> IsUserAdminAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == RoleConstants.AdminRoleId;
        }

        protected async Task<bool> IsUserSellerAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == RoleConstants.SellerRoleId;
        }

        protected async Task<bool> IsUserBuyerAsync()
        {
            var user = await GetCurrentUserAsync();

            return user?.RoleId == RoleConstants.BuyerRoleId;
        }
    }
}
