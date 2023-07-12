using API.DTO;
using API.DTO.OrderDTO;
using API.Response;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.Order;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public OrderController(IOrderService _orderService, UserManager<User> _userManager, IMapper _mapper)
        {
            orderService = _orderService;
            userManager = _userManager;
            mapper = _mapper;
        }

        [HttpGet("GetUserOrders")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUserOrders(Email email)
        {
            var user = await userManager.FindByEmailAsync(email.email);

            if (user == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found" }
                });
            }

            var order = await orderService.GetOrdersByUserAsync(email.email);

            return Ok(new APIResponse { Result = mapper.Map<List<OrderDTO>>(order) });
        }

        [HttpGet("GetAllOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetAllOrder(SearchString searchString)
        {
            var order = await orderService.GetAllOrdersAsync();

            if (searchString.Search != null)
            {
                var search = searchString.Search.Trim();
                order = order.Where(n => n.UserEmail.Contains(search) || n.Status.ToString().Contains(search)).ToList();
            }

            return Ok(new APIResponse { Result = mapper.Map<List<OrderDTO>>(order) });
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] ICollection<OrderItemDTO> orderItemsDTO, string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new APIResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "User not found" }
                });
            }
            List<OrderItem> orderItems = mapper.Map<List<OrderItem>>(orderItemsDTO);

            var order = await orderService.StoreOrderAsync(orderItems, user);

            return Ok(new APIResponse { Result = order });
        }

    }
}
