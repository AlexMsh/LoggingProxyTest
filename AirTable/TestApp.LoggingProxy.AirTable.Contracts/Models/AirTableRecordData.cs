using System;

namespace TestApp.LoggingProxy.AirTable.Contracts.Models
{
    public class AirTableRecordData : AirTableRecordFieldsData
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
