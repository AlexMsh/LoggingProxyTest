using System;

namespace TestApp.LoggingProxy.Contracts.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception baseException) : base(message, baseException)
        {
        }
    }
}
