using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Web.Models;
using Web.Models.DTO.OrderDTO;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        [Authorize]
        public async Task<IActionResult> Index(string? SearchString)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            List<OrderDTO> orders;

            if (userRole == "admin" || userRole == "employee")
            {
                orders = await orderService.GetAllOrder(SearchString, GetToken());
            }
            else
            {
                var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                orders = await orderService.GetUserOrder(userEmail, GetToken()); /// addd SearchString
            }

            return View(orders);
        }
        private string GetToken()
        {
            return HttpContext.Session.GetString(SD.SessionToken);
        }
    }
}
