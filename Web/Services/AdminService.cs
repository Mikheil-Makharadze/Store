using Newtonsoft.Json;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO.IdentityDTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private string APIUrl;
        public AdminService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }

        public async Task ChangeRole(UserWithRoleDTO Userinfo, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = Userinfo,
                Url = APIUrl + "/api/Admin/",
                Token = token
            });

            CheckAPIResponse(apiResponse);
        }

        public async Task DeleteUser(string email, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Data = new Email { email = email },
                Url = APIUrl + "/api/Admin",
                Token = token

            });

            CheckAPIResponse(apiResponse);
        }

        public async Task<List<UserWithRoleDTO>> GetAllUsers(string? searchString, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Admin/Users",
                Token = token,
                Data = new SearchString { Search = searchString }
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<List<UserWithRoleDTO>>(Convert.ToString(apiResponse.Result));
        }

        public async Task<UserWithRoleDTO> GetUser(string email, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Admin/User",
                Data = new Email { email = email},
                Token = token
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<UserWithRoleDTO>(Convert.ToString(apiResponse.Result));
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
