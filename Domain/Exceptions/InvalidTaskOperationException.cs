namespace Domain.Exceptions
{
    public class InvalidTaskOperationException : Exception
    {
        public InvalidTaskOperationException() : base() { }

        public InvalidTaskOperationException(string message) : base(message) { }

        public InvalidTaskOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
