namespace TestApp.LoggingProxy.AirTable.Contracts.Models
{
    public class AirTablePagedRecords<T> : AirTableRecords<T>
        where T : AirTableRecordFieldsData
    {
        public string Offset { get; set; }
    }
}
