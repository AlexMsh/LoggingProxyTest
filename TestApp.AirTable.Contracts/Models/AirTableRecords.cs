using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestApp.LoggingProxy.Contracts.Models.AirTable
{
    public class AirTableRecords<T> where T : AirTableRecordFieldsData
    {
        [JsonProperty("records")]
        public List<T> Records { get; set; }
    }

}
