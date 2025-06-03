
namespace Domain.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException() : base() { }

        public InvalidCurrencyException(string message) : base(message) { }

        public InvalidCurrencyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
