using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using TestApp.LoggingProxy.Contracts.Exceptions;

namespace TestApp.LoggingProxy.API.Filters
{
    public class LoggingProxyExceptionFIlter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // TODO: add logging
            //var logger = context.HttpContext.RequestServices.GetService<ILogger<LoggingProxyExceptionFIlter>>();
            if (context.Exception is DomainException || context.Exception.GetBaseException() is DomainException)
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (context.Exception is ArgumentNullException || context.Exception.GetBaseException() is ArgumentNullException)
            {
                context.Result = new BadRequestResult();
                return;
            }

            context.Result = new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }
}
