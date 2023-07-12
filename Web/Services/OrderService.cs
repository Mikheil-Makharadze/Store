using Newtonsoft.Json;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO.OrderDTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private string APIUrl;
        public OrderService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }
        public async Task CreateOrder(List<OrderItemDTO> orderItemsDTO, string email)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>> GetAllOrder(string? searchString, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Order/GetAllOrder",
                Token = token,
                Data = new SearchString { Search = searchString }
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(apiResponse.Result));
        }

        public async Task<List<OrderDTO>> GetUserOrder(string email, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Order/GetUserOrders",
                Data = new Email { email = email },
                Token = token
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(apiResponse.Result));
        }
        private void CheckAPIResponse(APIResponse apiResponse)
        {
            if (apiResponse.IsSuccess == false)
            {
                throw new APIException(apiResponse.StatusCode, apiResponse.ErrorMessages);
            }
        }
    }
}
