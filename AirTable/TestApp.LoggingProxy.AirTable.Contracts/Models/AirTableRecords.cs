using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestApp.LoggingProxy.AirTable.Contracts.Models
{
    public class AirTableRecords<T> where T : AirTableRecordFieldsData
    {
        [JsonProperty("records")]
        public List<T> Records { get; set; }
    }

}
