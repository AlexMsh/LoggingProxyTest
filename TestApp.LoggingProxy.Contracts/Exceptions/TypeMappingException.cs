using System;

namespace TestApp.LoggingProxy.Contracts.Exceptions
{
    public class TypeMappingException : DomainException
    {
        private const string DefaultErrorMessage = "Invalid Data format. Wasn't able to map types";

        public TypeMappingException() : base(DefaultErrorMessage)
        {
        }


        public TypeMappingException(Exception baseException) : base(DefaultErrorMessage, baseException)
        {
        }

        public TypeMappingException(string message) : base(message)
        {
        }

        public TypeMappingException(string message, Exception baseException) : base(message, baseException)
        {
        }
    }
}
