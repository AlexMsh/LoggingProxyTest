using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestApp.LoggingProxy.Contracts;
using TestApp.LoggingProxy.Contracts.Extensions.AirTable;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.Services.UnitTests
{

    public class LoggingProxyEnricherTests
    {
        protected DateTime _dateTime = DateTime.Now;
        protected static object[] LogRecords =
        {
            new LogRecord()
            {
                Text  = "Test log Record",
                Title = "Test log record message"
            },
            new LogRecord()
            {
                Text  = "Test log Record",
            },
            new LogRecord()
            {
                Title = "Test log record message"
            },
            new LogRecord()
            {
            }
        };

        public class PostMessageAsync : LoggingProxyEnricherTests
        {
            public PostMessageAsync()
            {
            }

            [TestCaseSource(nameof(LogRecords))]
            public async Task LogRecordPassed_SetsCorrectDate(LogRecord record)
            {
                //adjust
                var loggingProxyMock = new Mock<ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>>();
                var loggingProxyEnricher = new LoggingProxyEnricher(new DateTimeProvider(_dateTime), loggingProxyMock.Object);

                //act
                var enrichedLogRecord = await loggingProxyEnricher.PostMessageAsync(record);

                //assert
                loggingProxyMock.Verify(service => service.PostMessageAsync(It.Is<EnrichedLogRecord>(r => r.ReceivedAt == _dateTime)), Times.Once);
            }

            [TestCaseSource(nameof(LogRecords))]
            public async Task LogRecordPassed_SetsId(LogRecord record)
            {
                //adjust
                var loggingProxyMock = new Mock<ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>>();
                var loggingProxyEnricher = new LoggingProxyEnricher(new DateTimeProvider(_dateTime), loggingProxyMock.Object);

                //act
                var enrichedLogRecord = await loggingProxyEnricher.PostMessageAsync(record);

                //assert
                loggingProxyMock.Verify(service => service.PostMessageAsync(It.Is<EnrichedLogRecord>(r => !string.IsNullOrWhiteSpace(r.Id))), Times.Once);
            }

            [TestCaseSource(nameof(LogRecords))]
            public async Task LogRecordPassed_ExecutesProxyService(LogRecord record)
            {
                //adjust
                var loggingProxyMock = new Mock<ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>>();
                var loggingProxyEnricher = new LoggingProxyEnricher(new DateTimeProvider(_dateTime), loggingProxyMock.Object);
                
                //act
                var enrichedLogRecord = await loggingProxyEnricher.PostMessageAsync(record);

                //assert
                loggingProxyMock.Verify(p => p.PostMessageAsync(It.IsAny<EnrichedLogRecord>()), Times.Once);
            }

            [Test]
            public void LogRecordPassed_ExecutesProxyService()
            {
                //adjust
                LogRecord record = default;
                var loggingProxyMock = new Mock<ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>>();
                var loggingProxyEnricher = new LoggingProxyEnricher(new DateTimeProvider(_dateTime), loggingProxyMock.Object);

                //act
                Assert.ThrowsAsync<ArgumentNullException>(async () => await loggingProxyEnricher.PostMessageAsync(record));

                //assert
                loggingProxyMock.Verify(p => p.PostMessageAsync(It.IsAny<EnrichedLogRecord>()), Times.Never);
            }
        }
    }
}