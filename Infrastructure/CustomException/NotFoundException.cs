using System.Net;

namespace Infrastructure.CustomeException
{
    public class NotFoundException : Exception
    {
        // Status code property
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
