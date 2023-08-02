using API.Response;
using Infrastructure.CustomeException;
using Newtonsoft.Json;
using System.Net;

namespace API.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        #region Methods
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Call the next middleware in the pipeline
                await next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception and generate a response
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Create the APIResponse object
            var apiResponse = new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string>
                {
                    ex.Message,
                    ex.InnerException?.Message!
                }
            };

            if(ex is NotFoundException notFound)
            {
                apiResponse.StatusCode = notFound.StatusCode;
            }
            // Set the response status code
            context.Response.StatusCode = (int)apiResponse.StatusCode;
            context.Response.ContentType = "application/json";

            // Serialize the APIResponse object to JSON
            var json = JsonConvert.SerializeObject(apiResponse);

            // Write the JSON response to the HTTP response body
            await context.Response.WriteAsync(json);
        }
        #endregion
    }
}
