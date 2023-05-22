using Web.Models;
using Web.Models.DTO.IdentityDTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IHttpClientFactory clientFactory;
        private string APIUrl;
        public AccountService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            clientFactory = _clientFactory;
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }
        public async Task<APIResponse> LoginAsync(LoginDTO loginDto)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginDto,
                Url = APIUrl + "/api/Account/Login"
            });
        }

        public async Task<APIResponse> RegisterAsync(RegisterDTO registerDTO)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = registerDTO,
                Url = APIUrl + "/api/Account/Register"
            });
        }
    }
}
