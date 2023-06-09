using Newtonsoft.Json;
using System;
using Web.ExceptionFilter.Exceptions;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ProductService : GenericService<ProductDTO, ProductCreateDTO>, IProductService
    {
        protected static string Url = "/api/Product";
        private string APIUrl;
        public ProductService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(Url, _clientFactory, configuration)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI");
        }

        public async Task<List<ProductDTO>> GetAllDetailsAsync(string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/api/Product/AllDetails",
                Token = token
            });

            if (apiResponse.IsSuccess == false)
            {
                throw new APIException(apiResponse.StatusCode, apiResponse.ErrorMessages);
            }

            return JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(apiResponse.Result));
        }
    }
}
