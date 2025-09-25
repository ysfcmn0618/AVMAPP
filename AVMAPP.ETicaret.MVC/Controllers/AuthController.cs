using AutoMapper;
using AVMAPP.Models.DTo.Dtos;
using AVMAPP.Models.DTo.Models.Auth;
using AVMAPP.Models.DTO.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
namespace AVMAPP.ETicaret.MVC.Controllers
{
    [AllowAnonymous]
    public class AuthController(IMapper _mapper, IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            //Mapleme işlemleri lazım
            var user = _mapper.Map<UserDto>(newUser);

            var response = await Client.PostAsJsonAsync("api/Auth/register", user);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız. Lütfen tekrar deneyin.");
                return View(newUser);
            }
            else {
                SetSuccessMessage("Kayıt işlemi başarılı. Giriş yapabilirsiniz.");

                ModelState.Clear();
            }


            return RedirectToAction("Index", "Home");
        }


        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginViewModel loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var response = await Client.PostAsJsonAsync("api/Auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(loginDto);
            }

            var loginResult = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (loginResult == null || string.IsNullOrEmpty(loginResult.Token) || loginResult.User == null)
            {
                ModelState.AddModelError("", "Giriş işlemi başarısız oldu.");
                return View(loginDto);
            }

            // JWT token cookie
            Response.Cookies.Append("jwt_token", loginResult.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            // Kullanıcı adı ve ID cookie
            Response.Cookies.Append("name", loginResult.User.FirstName ?? loginResult.User.Email, new CookieOptions
            {
                HttpOnly = false, // Razor view okuyabilsin
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            Response.Cookies.Append("userId", loginResult.User.Id.ToString(), new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            await LogInAsync(loginResult.User);

            return RedirectToAction("Index", "Home");
        }



        [Route("forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var user = await _dbContext.FirstOrDefaultAsync(u => u.Email == model.Email);
            var response = await Client.GetAsync($"api/User/Email/{model.Email}");
            //if (user is null)
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            if (users == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }

            var user = users.Find(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(model);
            }
            // Şifre sıfırlama kodu oluşturulacak ve kullanıcıya mail gönderilecek...
            await SendResetPasswordEmailAsync(user);
            //ViewBag.SuccessMessage = "Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.";
            SetSuccessMessage("Şifre sıfırlama maili gönderildi. Lütfen e-posta adresinizi kontrol edin.");
            ModelState.Clear();
            return View();
        }

        [Route("renew-password/{verificationCode}")]
        [HttpGet]
        public async Task<IActionResult> RenewPasswordAsync([FromRoute] string verificationCode)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verificationCode))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            var response = await Client.GetAsync($"api/user/reset-password-token/{verificationCode}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var user = await response.Content.ReadFromJsonAsync<UserDto>();

            if (user is null)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            return View(new RenewPasswordViewModel
            {
                Email = user.Email,
                Token = verificationCode,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
            });
        }

        [Route("/renew-password")]
        [HttpPost]
        public async Task<IActionResult> RenewPasswordAsync([FromForm] RenewPasswordViewModel renewPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(renewPasswordModel);
            }

            var response = await Client.GetAsync($"api/user/reset-password-token/{renewPasswordModel.Token}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var user = await response.Content.ReadFromJsonAsync<UserDto>();

            if (user is null)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            user.Password = renewPasswordModel.Password;
            user.ResetPasswordToken = string.Empty;

            response = await Client.PutAsJsonAsync($"api/user/{user.Id}", user);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Şifre yenilemede bir hatayla karşılaşıldı.");
                return View();
            }

            SetSuccessMessage("Şifreniz başarıyla yenilendi. Giriş yapabilirsiniz.");
            return View();
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt_token");

            await LogoutUser();
            return RedirectToAction(nameof(Login));
        }
        private async Task LogInAsync(UserDto user)
        {
            var claims = new List<Claim>
    {
             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new Claim(ClaimTypes.Name, $"{user.FirstName ?? ""} {user.LastName ?? ""}".Trim()),
             new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
             new Claim(ClaimTypes.Role, (user.Role?.Name ?? "").Replace("buyer","Buyer").Replace("seller","Seller"))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }
        [NonAction]
        public async Task LogoutUser()
        {
            // TODO: Authorization implemente edildikten sonra bu metot tamamlanacak...

            RemoveCookie("userId");
            RemoveCookie("mail");
            RemoveCookie("name");
            RemoveCookie("surname");
            RemoveCookie("role");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        private async Task SendResetPasswordEmailAsync(UserDto user)
        {
            // Gönderici mail bilgileri güncellenmeli
            const string host = "smtp.gmail.com";
            const int port = 587;
            const string from = "mail";
            const string password = "şifre";

            var resetPasswordToken = Guid.NewGuid().ToString("n");
            user.ResetPasswordToken = resetPasswordToken;
            var response = await Client.PutAsJsonAsync($"api/user/{user.Id}", user);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            using SmtpClient client = new(host, port)
            {
                Credentials = new NetworkCredential(from, password)
            };

            MailMessage mail = new()
            {
                From = new MailAddress(from),
                Subject = "Şifre Sıfırlama",
                Body = $"Merhaba {user.FirstName}, <br> Şifrenizi sıfırlamak için <a href='https://localhost:5001/renew-password/{user.ResetPasswordToken}'>tıklayınız</a>.",
                IsBodyHtml = true,
            };

            mail.To.Add(user.Email);

            await client.SendMailAsync(mail);
        }

    }

}
