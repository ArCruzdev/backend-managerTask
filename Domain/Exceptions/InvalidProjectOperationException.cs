
namespace Domain.Exceptions
{
    public class InvalidProjectOperationException : Exception
    {
        public InvalidProjectOperationException() : base() { }
        public InvalidProjectOperationException(string message) : base(message) { }
        public InvalidProjectOperationException(string message, Exception innerException)
        : base(message, innerException) { }
    }
}
