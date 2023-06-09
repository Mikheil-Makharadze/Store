using System.Runtime.Serialization;

namespace Web.ExceptionFilter.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public string ErrorMessage { get; set; }

        public BadRequestException(string? message) : base(message)
        {
            ErrorMessage = message;
        }
    }
}