using System;

namespace Application.Common.Exceptions
{
    // Excepción para representar un error de solicitud inválida (HTTP 400 Bad Request)
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
