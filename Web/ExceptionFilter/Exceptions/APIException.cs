using System.Net;

namespace Web.ExceptionFilter.Exceptions
{
    public class APIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
        public APIException(HttpStatusCode StatusCode, List<string> ErrorMessages) : base(ErrorMessages.FirstOrDefault())
        {
            this.StatusCode = StatusCode;
            this.ErrorMessages = ErrorMessages;
        }
    }
}