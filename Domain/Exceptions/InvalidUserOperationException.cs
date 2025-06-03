using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidUserOperationException : Exception
    {
        public InvalidUserOperationException() : base() { }

        public InvalidUserOperationException(string message) : base(message) { }

        public InvalidUserOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
