using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models.DTO;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class ProducerController : Controller
    {
        public IProducerService producerService;
        public ProducerController(IProducerService _producerService)
        {
            producerService = _producerService;
        }
        public async Task<IActionResult> Index()
        {
            var response = await producerService.GetAllAsync(HttpContext.Session.GetString(SD.SessionToken));
            var list = JsonConvert.DeserializeObject<List<ProducerDTO>>(Convert.ToString(response.Result));

            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await producerService.GetByIdAsync(id,HttpContext.Session.GetString(SD.SessionToken));
            var list = JsonConvert.DeserializeObject<ProducerDTO>(Convert.ToString(response.Result));

            return View(list);
        }
    }
}
