﻿using Newtonsoft.Json.Linq;
using Web.Models;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ProductService : GenericService, IProductService
    {
        protected static string Url = "/api/Product";
        public ProductService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(Url, _clientFactory, configuration)
        {
        }
    }
}
