namespace TestApp.LoggingProxy.Contracts.Models.AirTable
{
    public class AirTablePagedRecords<T> : AirTableRecords<T>
        where T : AirTableRecordFieldsData
    {
        public string Offset { get; set; }
    }
}
