using Newtonsoft.Json;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO.IdentityDTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private string APIUrl;
        public AccountService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }
        public async Task<UserDTO> LoginAsync(LoginDTO loginDto)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginDto,
                Url = APIUrl + "/api/Account/Login"
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(apiResponse.Result));

        }

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = registerDTO,
                Url = APIUrl + "/api/Account/Register"
            });

            CheckAPIResponse(apiResponse);
        }

        private void CheckAPIResponse(APIResponse apiResponse)
        {
            if(apiResponse.IsSuccess == false)
            {
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(apiResponse.ErrorMessages.FirstOrDefault());
                }
                else
                {
                    throw new APIException(apiResponse.StatusCode, apiResponse.ErrorMessages);
                }
            }
        }
    }
}
