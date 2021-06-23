using AutoMapper;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.LoggingProxy.AirTable.Contracts.Models;
using TestApp.LoggingProxy.AirTable.Services.Extensions;
using TestApp.LoggingProxy.Contracts;
using TestApp.LoggingProxy.Contracts.Exceptions;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.AirTable.Services
{
    public class AirTableProxyService : ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>
    {
        private const string SaveLogUrl = "v0/appD1b1YjWoXkUJwR/Messages";
        private const string GetLogsUrl = "v0/appD1b1YjWoXkUJwR/Messages?view=Grid%20view";

        private readonly IMapper _mapper;
        private readonly ILogger<AirTableProxyService> _logger;
        private readonly IRequestBuilder _requestBuilder;

        public AirTableProxyService(IRequestBuilder requestBuilder, IMapper mapper, ILogger<AirTableProxyService> logger)
        {
            _requestBuilder = requestBuilder;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EnrichedLogRecord>> GetMessagesAsync()
        {
            var (client, request) = _requestBuilder.BuildRequest(Method.GET, GetLogsUrl);
            var result = new List<EnrichedLogRecord>();

            var offset = await GetMessagesBatch(result, client, request);
            while (!string.IsNullOrWhiteSpace(offset))
            {
                request = new RestRequest($"{GetLogsUrl}&offset={offset}", Method.GET);
                offset = await GetMessagesBatch(result, client, request);
            }

            return result;
        }

        public async Task<IEnumerable<EnrichedLogRecord>> PostMessageAsync(EnrichedLogRecord record)
        {
            if (record == default(EnrichedLogRecord))
            {
                throw new ArgumentNullException(nameof(record));
            }

            var body = _mapper.SafeMap<EnrichedLogRecord, AirTableRecords<AirTableRecordFieldsData>>(record, _logger);
            var (client, request) = _requestBuilder.BuildRequest(Method.POST, SaveLogUrl, body);

            var airTableRecords = default(AirTableRecords<AirTableRecordData>);
            var response = await client.ExecutePostTaskAsync<AirTableRecords<AirTableRecordData>>(request);

            if (response?.IsSuccessful != true)
            {
                _logger.LogError(
                    "could not push a message to AirTable code: {errorCode}, message: {errorMessage}",
                    response.StatusCode,
                    response.ErrorMessage);

                throw new LogSendingException();
            }

            airTableRecords = response.Data;
            return airTableRecords?.Records
                    ?.Select(r => _mapper.SafeMap<AirTableRecordData, EnrichedLogRecord>(r))
                    ?.ToList();
        }

        private async Task<string> GetMessagesBatch(List<EnrichedLogRecord> result, RestClient client, RestRequest request)
        {
            var response = await client.ExecuteGetTaskAsync<AirTablePagedRecords<AirTableRecordData>>(request);
            if(response?.IsSuccessful != true)
            {
                _logger.LogError(
                    "could not get message batch from AirTable code: {errorCode}, message: {errorMessage}",
                    response.StatusCode,
                    response.ErrorMessage);

                throw new LogSendingException();
            }

            var airTablePagedRecords = response.Data;
            var tmpRecords = airTablePagedRecords?.Records?.Select(r => _mapper.SafeMap<AirTableRecordData, EnrichedLogRecord>(r));

            if (tmpRecords?.Any() == true)
            {
                result.AddRange(tmpRecords);
            }

            return airTablePagedRecords?.Offset;
        }
    }
}
