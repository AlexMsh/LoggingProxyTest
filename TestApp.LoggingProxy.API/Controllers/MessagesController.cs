using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.LoggingProxy.Contracts;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.API
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILoggingProxyService<LogRecord, EnrichedLogRecord> _loggingProxyService;
        public MessagesController(ILoggingProxyService<LogRecord, EnrichedLogRecord> loggingProxyService)
        {
            _loggingProxyService = loggingProxyService;
        }

        [HttpGet]
        public async Task<IEnumerable<EnrichedLogRecord>> GetProxiedMessages()
        {
            return await _loggingProxyService.GetMessagesAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<EnrichedLogRecord>> ProxyMessages(LogRecord logRecord)
        {
            return await _loggingProxyService.PostMessageAsync(logRecord);
        }
    }
}
