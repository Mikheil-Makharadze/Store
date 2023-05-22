using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        public IProductService productService;
        public IProducerService producerService;
        public ICategoryService categoryService;
        public ProductController(IProductService _productService, IProducerService _producerService, ICategoryService _categoryService)
        {
                productService = _productService;
                producerService = _producerService;
                categoryService = _categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await productService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));

            return View(list);
        }
        public async Task<IActionResult> Details(int id)
        {
            var response = await productService.GetByIdAsync(id,HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

            
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var response = await producerService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var producerlist = JsonConvert.DeserializeObject<List<ProducerDTO>>(Convert.ToString(response.Result));

            var response1 = await categoryService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var categorylist = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response1.Result));

            ViewBag.Producers = new SelectList(producerlist, "Id", "Name");
            ViewBag.Categoryes = new SelectList(categorylist, "Id", "Name");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProducerCreateDTO product)
        {
            var response = await productService.AddAsync(product, HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var response = await producerService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var producerlist = JsonConvert.DeserializeObject<List<ProducerDTO>>(Convert.ToString(response.Result));

            var response1 = await categoryService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var categorylist = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response1.Result));

            ViewBag.Producers = new SelectList(producerlist, "Id", "Name");
            ViewBag.Categoryes = new SelectList(categorylist, "Id", "Name");


            var response2 = await productService.GetByIdAsync(id, HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response2.Result));

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO product)
        {
            var response = await productService.UpdateAsync(product, HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var response = await productService.GetByIdAsync(id, HttpContext.Session.GetString(SD.SessionToken));

            var list = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await productService.DeleteAsync(id, HttpContext.Session.GetString(SD.SessionToken));

            return RedirectToAction("Index");
        }
    }
}
