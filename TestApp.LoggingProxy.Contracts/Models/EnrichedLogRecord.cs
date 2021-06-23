using System;

namespace TestApp.LoggingProxy.Contracts.Models
{
    public class EnrichedLogRecord : LogRecord
    {
        public string Id { get; set; }

        public DateTime ReceivedAt { get; set; }

    }
}
