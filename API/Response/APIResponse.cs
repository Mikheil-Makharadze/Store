﻿using System.Net;
using System.Text.Json;

namespace API.Response
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object? Result { get; set; }
    }
}
