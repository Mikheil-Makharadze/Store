﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Models.DTO;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class ProducerService : GenericService<ProducerDTO, ProducerCreateDTO>, IProducerService
    {
        private static string Url = "/api/Producer";
        public ProducerService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(Url, _clientFactory, configuration)
        {
        }
    }
}
