using System;

namespace TestApp.LoggingProxy.Contracts.Exceptions
{
    public class LogSendingException : DomainException
    {
        private const string DefaultErrorMessage = "Could not send log record into the destination service";

        public LogSendingException() : base(DefaultErrorMessage)
        {
        }

        public LogSendingException(Exception baseException) : base(DefaultErrorMessage, baseException)
        {
        }

        public LogSendingException(string message) : base(message)
        {
        }

        public LogSendingException(string message, Exception baseException) : base(message, baseException)
        {
        }
    }
}
