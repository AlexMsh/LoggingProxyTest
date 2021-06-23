using System;

namespace TestApp.LoggingProxy.Services
{
    public class DateTimeProvider
    {
        private readonly DateTime? _mock;
        public DateTimeProvider(DateTime? mock = null)
        {
            _mock = mock;
        }

        public DateTime GetTime() => _mock ?? DateTime.Now;

        public DateTime GetUtcTime() => _mock ?? DateTime.UtcNow;
    }
}
