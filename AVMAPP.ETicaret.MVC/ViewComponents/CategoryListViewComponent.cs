using AutoMapper;
using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Models.DTo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace AVMAPP.ETicaret.MVC.ViewComponents
{
    public class CategoryListViewComponent(IMapper mapper,IHttpClientFactory _httpClient) : ViewComponent
    {
        private HttpClient Client => _httpClient.CreateClient("ApiClient");

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await Client.GetFromJsonAsync<List<CategoryDto>>("api/Category");
            var modelCategory=mapper.Map<List<CategoryListViewModel>>(categories);
            return View(modelCategory);
        }
    }
}
