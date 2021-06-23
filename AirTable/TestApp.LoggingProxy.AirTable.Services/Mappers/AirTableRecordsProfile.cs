using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TestApp.LoggingProxy.AirTable.Contracts.Models;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.AirTable.Services.Mappers
{
    public class AirTableRecordsProfile : Profile
    {
        public AirTableRecordsProfile()
        {
            CreateMap<EnrichedLogRecord, AirTableRecords<AirTableRecordFieldsData>>()
                .BeforeMap((src, dest) =>
                {
                    dest.Records = dest.Records ?? new List<AirTableRecordFieldsData>();
                    dest.Records.Add(new AirTableRecordFieldsData()
                    {
                        Fields = new AirTableRecordFields()
                        {
                            Message = src.Text,
                            Summary = src.Title,
                            Id = src.Id,
                            ReceivedAt = src.ReceivedAt,
                        },
                    });
                });

            CreateMap<List<EnrichedLogRecord>, AirTableRecords<AirTableRecordFieldsData>>()
                .BeforeMap((src, dest) =>
                {
                    dest.Records = dest.Records ?? new List<AirTableRecordFieldsData>();
                    dest.Records.AddRange(src.Select(record => new AirTableRecordFieldsData()
                    {
                        Fields = new AirTableRecordFields()
                        {
                            Message = record.Text,
                            Summary = record.Title,
                            Id = record.Id,
                            ReceivedAt = record.ReceivedAt,
                        },
                    }));
                });
        }
    }
}
