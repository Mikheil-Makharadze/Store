using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ICategoryService categoryService;
        public readonly IProductService productService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService _categoryService, IProductService _productService, IMapper _mapper)
        {
            categoryService = _categoryService;
            productService = _productService;
            mapper = _mapper;   
        }
        public async Task<IActionResult> Index()
        {
            var categorys = await categoryService.GetAllDetailsAsync(GetToken());

            return View(categorys);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Create()
        {
            await UpdataProductViewBag();

            return View();
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.AddAsync(category, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataProductViewBag();

            return View(category);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Edit(int id)
        {
            await UpdataProductViewBag();

            var category = mapper.Map<CategoryUpdateDTO>(await categoryService.GetByIdAsync(id, GetToken()));

            return View(category);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateDTO category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.UpdateAsync(category.Id, category, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataProductViewBag();

            return View(category);

        }
        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetByIdAsync(id, GetToken());

            return View(category);
        }
        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoryService.DeleteAsync(id, GetToken());

            return RedirectToAction("Index");
        }

        #region PrivateMethods
        private string GetToken()
        {
            return HttpContext.Session.GetString(SD.SessionToken);
        }

        private async Task UpdataProductViewBag()
        {
            var products = await productService.GetAllAsync(GetToken());

            ViewBag.Products = new SelectList(products, "Id", "Name");
        }
        #endregion
    }
}
