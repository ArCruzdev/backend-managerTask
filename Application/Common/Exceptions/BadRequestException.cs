using System;

namespace Application.Common.Exceptions
{
    // Exception to represent a bad request error (HTTP 400 Bad Request)
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base() { }

        public BadRequestException(string message)
            : base(message) { }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
