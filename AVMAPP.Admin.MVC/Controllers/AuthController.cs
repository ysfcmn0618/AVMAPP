using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Models.DTo.Models.Auth;
using AVMAPP.Models.DTO.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AVMAPP.Admin.MVC.Controllers
{
    [AllowAnonymous]
    [Route("/auth")]
    public class AuthController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);

            // API'ye login isteği gönder
            var response = await Client.PostAsJsonAsync("api/auth/login", loginModel);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(loginModel);
            }

            // API’den AuthResponseDto oku
            var loginResult = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (loginResult == null || loginResult.User == null || string.IsNullOrEmpty(loginResult.Token))
            {
                ModelState.AddModelError(string.Empty, "Giriş işlemi başarısız.");
                return View(loginModel);
            }

            // Admin rol kontrolü
            if (loginResult.User.Role?.Name?.ToLower() != "admin")
            {
                ModelState.AddModelError(string.Empty, "Bu sayfaya erişim yetkiniz yok.");
                return View(loginModel);
            }
            HttpContext.Session.SetString("AccessToken", loginResult.Token);
            // Cookie Authentication ile oturum aç
            await DoLoginAsync(loginResult.User);

            // ReturnUrl varsa ve yerel ise yönlendir
            if (Request.Query.ContainsKey("ReturnUrl"))
            {
                var returnUrl = Request.Query["ReturnUrl"]!.ToString();
                if (Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("logout")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await DoLogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        private async Task DoLoginAsync(UserDto user)
        {
            if (user == null)
            {
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
               new Claim(ClaimTypes.Role,user.Role?.Name?.Equals("admin", StringComparison.OrdinalIgnoreCase) == true ? "Admin" : (user.Role?.Name ?? "Undefined"))};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            };
            HttpContext.Session.Remove("AccessToken");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        }

        private async Task DoLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
