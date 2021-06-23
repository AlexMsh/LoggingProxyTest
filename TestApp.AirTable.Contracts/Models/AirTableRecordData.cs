using System;

namespace TestApp.LoggingProxy.Contracts.Models.AirTable
{
    public class AirTableRecordData : AirTableRecordFieldsData
    {
        public string Id { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
