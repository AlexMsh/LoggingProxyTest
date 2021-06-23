using Newtonsoft.Json;

namespace TestApp.LoggingProxy.AirTable.Contracts.Models
{
    public class AirTableRecordFieldsData
    {
        [JsonProperty("fields")]
        public AirTableRecordFields Fields { get; set; }
    }
}
