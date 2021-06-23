using Newtonsoft.Json;
using TestApp.LoggingProxy.Contracts.Models.AirTable;

namespace TestApp.LoggingProxy.Contracts.Models
{
    public class AirTableRecordFieldsData
    {
        [JsonProperty("fields")]
        public AirTableRecordFields Fields { get; set; }
    }
}
