using Newtonsoft.Json;
using System;

namespace TestApp.LoggingProxy.Contracts.Models.AirTable
{
    public class AirTableRecordFields
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string Summary { get; set; }

        public string Message { get; set; }

        [JsonProperty("receivedAt")]
        public DateTime ReceivedAt { get; set; }
    }
}
