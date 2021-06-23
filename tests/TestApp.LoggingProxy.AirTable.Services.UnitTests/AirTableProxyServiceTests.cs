using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.LoggingProxy.AirTable.Contracts.Configuration;
using TestApp.LoggingProxy.AirTable.Contracts.Models;
using TestApp.LoggingProxy.AirTable.Services;
using TestApp.LoggingProxy.Contracts.Exceptions;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.AirTable.Services.Tests
{
    public class AirTableProxyServiceTests
    {
        protected AirTableProxyService _service;
        protected Mock<RestClient> _restClientMock;
        protected Mock<IRestResponse<AirTableRecords<AirTableRecordData>>> _restResponseMock;
        protected Mock<IRequestBuilder> _requestBuilderMock;
        protected Mock<IMapper> _mapperMock;
        protected Mock<IOptions<AirTableConfiguration>> _settingsMock;
        protected Mock<ILogger<AirTableProxyService>> _loggerMock;

        //TODO: tests for GetMessagesAsync should go here, skip due to time constraints

        public class PostMessageAsync : AirTableProxyServiceTests
        {
            private AirTableRecordData _testExternalServiceResponse = new AirTableRecordData()
            {
                CreatedTime = DateTime.Now,
                Fields = new AirTableRecordFields()
                {
                    Message = "test message",
                    Summary = "test title",
                    ReceivedAt = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                }
            };

            private EnrichedLogRecord _testInputEnrichedLogRecord = new EnrichedLogRecord()
            {
                ReceivedAt = DateTime.Now,
                Text = "test message",
                Title = "test title",
                Id = Guid.NewGuid().ToString()
            };

            [SetUp]
            public void SetUp()
            {
                _restClientMock = new Mock<RestClient>();
                _restResponseMock = new Mock<IRestResponse<AirTableRecords<AirTableRecordData>>>();
                _requestBuilderMock = new Mock<IRequestBuilder>();
                _mapperMock = new Mock<IMapper>();
                _settingsMock = new Mock<IOptions<AirTableConfiguration>>();
                _loggerMock = new Mock<ILogger<AirTableProxyService>>();

                _restClientMock
                    .Setup(service => service
                        .ExecutePostTaskAsync<AirTableRecords<AirTableRecordData>>(It.IsAny<IRestRequest>()))
                    .ReturnsAsync(_restResponseMock.Object);

                _requestBuilderMock
                    .Setup(service => service.BuildRequest(It.IsAny<Method>(), It.IsAny<string>(), It.IsAny<object>()))
                    .Returns((_restClientMock.Object, new Mock<RestRequest>().Object));

                _restResponseMock
                    .Setup(response => response.IsSuccessful)
                    .Returns(true);

                _restResponseMock
                   .Setup(response => response.Data)
                   .Returns(new AirTableRecords<AirTableRecordData>()
                   {
                       Records = new List<AirTableRecordData>() { _testExternalServiceResponse }
                   });

                _service = new AirTableProxyService(_requestBuilderMock.Object, _mapperMock.Object, _loggerMock.Object);
            }

            [Test]
            public void NullRecordPassed_ShouldThrow_ArgumentNullException() => 
                Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.PostMessageAsync(default(EnrichedLogRecord)));

            [Test]
            public async Task NoDataFromExternalService_MappingNotExecuted()
            {
                //adjust
                _restResponseMock
                   .Setup(response => response.Data)
                   .Returns(default(AirTableRecords<AirTableRecordData>));

                //act
                var records = await _service.PostMessageAsync(new EnrichedLogRecord()
                {
                    Text = "test log message",
                    Title = "test log",
                    ReceivedAt = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                });

                //assert
                _mapperMock.Verify(s => s.Map<EnrichedLogRecord, AirTableRecords<AirTableRecordFieldsData>>(It.IsAny<EnrichedLogRecord>()), Times.Never);
            }

            [Test]
            public async Task ValidDataFromExternalService_MappingIsExecuted()
            {
                //act
                var records = await _service.PostMessageAsync(new EnrichedLogRecord()
                {
                    Text = "test log message",
                    Title = "test log",
                    ReceivedAt = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                });

                //assert
                _mapperMock.Verify(s => s.Map<EnrichedLogRecord>(It.IsAny<object>()), Times.Once);
            }

            [Test]
            public void ExternalServiceRequestFailed_ShouldThrow_LogSendingException()
            {
                //adjust
                _restResponseMock
                    .Setup(response => response.IsSuccessful)
                    .Returns(false);

                //act / assert
                Assert.ThrowsAsync<LogSendingException>(async () => await _service.PostMessageAsync(_testInputEnrichedLogRecord));
            }

            [Test]
            public void MappingFails_ShouldThrow_TypeMappingException()
            {
                //adjust
                _mapperMock
                    .Setup(s => s.Map<AirTableRecords<AirTableRecordFieldsData>>(It.IsAny<EnrichedLogRecord>()))
                    .Throws(new Exception());

                //act / assert
                Assert.ThrowsAsync<TypeMappingException>(async () => await _service.PostMessageAsync(_testInputEnrichedLogRecord));
            }
        }
    }
}