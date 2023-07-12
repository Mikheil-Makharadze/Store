using Web.Models.DTO;
using Web.Models;
using Web.Services.Interfaces;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.ExceptionFilter.Exceptions;

namespace Web.Services
{
    public class GenericService<T, TCreate> : BaseService, IGenericService<T,TCreate>
    {
        private string APIUrl;
        public GenericService(string URL, IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            APIUrl = configuration.GetValue<string>("ServiceUrls:StoreAPI") + URL;
        }

        public async Task AddAsync(object entity, string token)
        {
            var apiResponse =  await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = APIUrl,
                Data = entity,
                Token = token
            });

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(apiResponse.ErrorMessages.FirstOrDefault());
            }

            CheckAPIResponse(apiResponse);
        }

        public async Task DeleteAsync(int Id, string token)
        {
            var apiResponse =  await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = APIUrl + "/" + Id,
                Token = token
            });

            CheckAPIResponse(apiResponse);
        }

        public async Task<List<T>> GetAllAsync(string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl,
                Token = token
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<List<T>>(Convert.ToString(apiResponse.Result));
        }

        public async Task<List<T>> GetAllDetailsAsync(string? searchString, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/AllDetails",
                Token = token,
                Data = new SearchString { Search = searchString }
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<List<T>>(Convert.ToString(apiResponse.Result));
        }

        public async Task<T> GetByIdAsync(int Id, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = APIUrl + "/" + Id,
                Token = token
            });

            CheckAPIResponse(apiResponse);

            return JsonConvert.DeserializeObject<T>(Convert.ToString(apiResponse.Result));
        }

        public async Task UpdateAsync(int Id, object entity, string token)
        {
            var apiResponse = await SendAsync(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = APIUrl + "/" + Id,
                Data = entity,
                Token = token
            });

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new BadRequestException(apiResponse.ErrorMessages.FirstOrDefault());
            }

            CheckAPIResponse(apiResponse);
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
