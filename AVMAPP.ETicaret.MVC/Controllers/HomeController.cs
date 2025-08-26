using System.Diagnostics;
using AutoMapper;
using AVMAPP.ETicaret.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.ETicaret.MVC.Controllers
{
    public class HomeController(IMapper mapper,IHttpClientFactory httpClient) : Controller
    {      
        private HttpClient Client => httpClient.CreateClient("ApiClient");
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
