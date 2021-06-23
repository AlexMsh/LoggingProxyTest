using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.LoggingProxy.Contracts;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.Services
{
    public class LoggingProxyEnricher : ILoggingProxyService<LogRecord, EnrichedLogRecord>
    {
        private readonly DateTimeProvider _timeProvider;
        private readonly ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord> _loggingProxyService;

        public LoggingProxyEnricher(DateTimeProvider timeProvider, ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord> loggingProxyService)
        {
            _timeProvider = timeProvider;
            _loggingProxyService = loggingProxyService;
        }
        public async Task<IEnumerable<EnrichedLogRecord>> GetMessagesAsync() => await _loggingProxyService.GetMessagesAsync();

        public Task<IEnumerable<EnrichedLogRecord>> PostMessageAsync(LogRecord record)
        {
            if(record == default(LogRecord))
            {
                throw new ArgumentNullException(nameof(record));
            }

            var enrichedRecord = new EnrichedLogRecord()
            {
                ReceivedAt = _timeProvider.GetUtcTime(),
                Id = Guid.NewGuid().ToString(),
                Text = record.Text,
                Title = record.Title
            };

            return _loggingProxyService.PostMessageAsync(enrichedRecord);
        }
    }
}
