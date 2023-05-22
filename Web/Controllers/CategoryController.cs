using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService categoryService;
        public CategoryController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await categoryService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var list = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));

            return View(list);
        }

    }
}
