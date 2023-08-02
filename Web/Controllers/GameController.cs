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
    public class GameController : Controller
    {
        public readonly IProductService productService;
        public readonly IProducerService producerService;
        public readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public GameController(IProductService _productService, IProducerService _producerService, ICategoryService _categoryService, IMapper _mapper)
        {
            productService = _productService;
            producerService = _producerService;
            categoryService = _categoryService;
            mapper = _mapper;
        }
        public async Task<IActionResult> Index(string? SearchString)
        {

            var products = await productService.GetAllDetailsAsync(SearchString, GetToken());

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await productService.GetByIdAsync(id, GetToken());

            return View(product);
        }       

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Create()
        {
            await UpdataCategoryViewBag();
            await UpdataProducerViewBag();

            return View();
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await productService.AddAsync(product, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataCategoryViewBag();
            await UpdataProducerViewBag();

            return View(product);         
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Edit(int id)
        {
            await UpdataCategoryViewBag();
            await UpdataProducerViewBag();

            var product = mapper.Map<ProductUpdateDTO>( await productService.GetByIdAsync(id, GetToken()));

            
            return View(product);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateDTO product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await productService.UpdateAsync(product.Id, product, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataCategoryViewBag();
            await UpdataProducerViewBag();

            return View(product); 
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productService.GetByIdAsync(id,GetToken());

            return View(product);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await productService.DeleteAsync(id,GetToken());

            return RedirectToAction("Index");
        }

        #region PrivateMethods
        private string GetToken()
        {
            return HttpContext.Session.GetString(SD.SessionToken)!;
        }

        private async Task UpdataCategoryViewBag()
        {
            var categorys = await categoryService.GetAllAsync(GetToken());

            ViewBag.Categoryes = new SelectList(categorys, "Id", "Name");
        }

        private async Task UpdataProducerViewBag()
        {
            var producers = await producerService.GetAllAsync(GetToken());

            ViewBag.Producers = new SelectList(producers, "Id", "Name");
        }
        #endregion
    }
}
