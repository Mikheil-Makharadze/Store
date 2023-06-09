using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class CategoryService : GenericService<CategoryDTO, CategoryCreateDTO>, ICategoryService
    {
        private static string Url = "/api/Category";
        private string APIUrl;
        public CategoryService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(Url, _clientFactory, configuration)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }

        public async Task<List<CategoryDTO>> GetAllDetailsAsync(string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Category/AllDetails",
                Token = token
            });

            if (apiResponse.IsSuccess == false)
            {
                throw new APIException(apiResponse.StatusCode, apiResponse.ErrorMessages);
            }

            return JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(apiResponse.Result));
        }
    }
}
