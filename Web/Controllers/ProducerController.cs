using Microsoft.AspNetCore.Mvc;
using Web.Models.DTO;
using Web.Models;
using Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Web.ExceptionFilter.Exceptions;

namespace Web.Controllers
{
    public class ProducerController : Controller
    {
        public readonly IProducerService producerService;
        public readonly IProductService productService;
        private readonly IMapper mapper;

        public ProducerController(IProducerService _producerService, IProductService _productService, IMapper _mapper)
        {
            producerService = _producerService;
            productService = _productService;
            mapper = _mapper;
        }
        public async Task<IActionResult> Index(string? SearchString)
        {
            var Producers = await producerService.GetAllDetailsAsync(SearchString, GetToken());

            return View(Producers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var producer = await producerService.GetByIdAsync(id, GetToken());

            return View(producer);
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
        public async Task<IActionResult> Create(ProducerCreateDTO producer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await producerService.AddAsync(producer, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataProductViewBag(); ;

            return View(producer);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Edit(int id)
        {
            await UpdataProductViewBag();

            var Producer = mapper.Map<ProducerUpdateDTO>(await producerService.GetByIdAsync(id, GetToken()));

            return View(Producer);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProducerUpdateDTO producer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await producerService.UpdateAsync(producer.Id, producer, GetToken());

                    return RedirectToAction("Index");
                }
                catch (BadRequestException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await UpdataProductViewBag();

            return View(producer);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        public async Task<IActionResult> Delete(int id)
        {
            var producer=  await producerService.GetByIdAsync(id, GetToken());

            return View(producer);
        }

        [Authorize(Policy = "AdminOrEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await producerService.DeleteAsync(id, GetToken());

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
