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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(string email)
        {
            try
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

                var order = orderService.GetOrdersByUserAsync(email);

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = mapper.Map<List<OrderDTO>>(order)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] ICollection<OrderItemDTO> orderItemsDTO, string email)
        {
            try
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

                var order = orderService.StoreOrderAsync(orderItems, user);

                return Ok(new APIResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccess = true,
                    Result = order
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.ToString() }
                });
            }
        }

    }
}
