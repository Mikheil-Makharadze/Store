using Web.Models.DTO;
using Web.Models;
using Web.Services.Interfaces;
using System;

namespace Web.Services
{
    public class GenericService : BaseService, IGenericService
    {
        private string APIUrl;
        public GenericService(string URL, IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI") + URL;
        }

        public async Task<APIResponse> AddAsync(object entity, string token)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = APIUrl,
                Data = entity,
                Token = token
            });
        }

        public async Task<APIResponse> DeleteAsync(int Id, string token)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = APIUrl + "/" + Id,
                Token = token
            });
        }

        public async Task<APIResponse> GetAllAsync(string token)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl,
                Token = token
            });
        }

        public async Task<APIResponse> GetByIdAsync(int Id, string token)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/" + Id,
                Token = token
            }); ;
        }

        public async Task<APIResponse> UpdateAsync(object entity, string token)
        {
            return await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = APIUrl,
                Data = entity,
                Token = token
            });
        }
    }
}
