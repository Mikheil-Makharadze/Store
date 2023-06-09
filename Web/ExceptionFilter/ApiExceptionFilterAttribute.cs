using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using Web.ExceptionFilter.Exceptions;

namespace Web.ExceptionFilter
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is BadRequestException) 
            {
                BadRequestException badRequestException = (BadRequestException)context.Exception;

                context.ModelState.AddModelError("", badRequestException.Message);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is APIException)
            {
                APIException apiException = (APIException)context.Exception;


                context.ExceptionHandled = true;
                context.Result = new ViewResult
                {
                    ViewName = "APIError", // Set the name of your error view
                    StatusCode = (int)apiException.StatusCode, // Set the desired HTTP status code
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                    {
                        // Pass any additional data to the error view if needed
                        { "ErrorMessage", apiException.Message }
                    }
                };

                // Add more conditions for other status codes if needed
            
                // Set the exception as handled
                context.ExceptionHandled = true;
            }

        }
    }
}
