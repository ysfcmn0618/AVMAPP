using AVMAPP.Data.APi.Models.Dtos;
using AVMAPP.Models.DTo.Models.ViewModels;
using AVMAPP.Models.DTO.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AVMAPP.Admin.MVC.Controllers
{
    [Route("/categories")]
    [Authorize(Roles = "admin")]
    public class CategoryController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("ApiClient");
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await Client.GetAsync("api/category");

            List<CategoryListViewModel> model = [];

            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            model = (await response.Content.ReadFromJsonAsync<List<CategoryListViewModel>>())
                ?? [];

            return View(model);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SaveCategoryViewModel newCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategoryModel);
            }

            var categoryDto = new CategoryDto
            {
                Name = newCategoryModel.Name,
                Color = newCategoryModel.Color,
                Icon = string.Empty,
            };

            var response = await Client.PostAsJsonAsync("api/category", categoryDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori oluşturulurken bir hata oluştu.");
                return View(newCategoryModel);
            }

            ViewBag.SuccessMessage = "Kategori başarıyla oluşturuldu.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/edit")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int categoryId)
        {

            var response = await Client.GetAsync($"api/category/{categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();

            if (category is null)
            {
                return NotFound();
            }

            var editCategoryModel = new SaveCategoryViewModel
            {
                Name = category.Name,
                Color = category.Color,
                Icon = category.Icon
            };

            return View(editCategoryModel);
        }

        [Route("{categoryId:int}/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int categoryId, [FromForm] SaveCategoryViewModel editCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategoryModel);
            }

            var response = await Client.GetAsync($"api/category/{categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var category = await response.Content.ReadFromJsonAsync<CategoryDto>();
            if (category is null)
            {
                return NotFound();
            }

            category.Name = editCategoryModel.Name;
            category.Color = editCategoryModel.Color;
            category.Icon = editCategoryModel.Icon ?? string.Empty;

            var updateResponse = await Client.PutAsJsonAsync($"api/category/{categoryId}", category);

            if (!updateResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori güncellenirken bir hata oluştu.");
                return View(editCategoryModel);
            }

            ViewBag.SuccessMessage = "Kategori başarıyla güncellendi.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int categoryId)
        {
            var response = await Client.DeleteAsync($"api/category/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(List));
        }
    }
}
