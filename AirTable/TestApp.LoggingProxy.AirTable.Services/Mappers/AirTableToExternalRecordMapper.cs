using AutoMapper;
using TestApp.LoggingProxy.AirTable.Contracts.Models;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.AirTable.Services.Mappers
{
    public class AirTableRecordDataProfile : Profile
    {
        public AirTableRecordDataProfile()
        {
            CreateMap<AirTableRecordData, EnrichedLogRecord>()
                .BeforeMap((src, dest) =>
                {
                    src.Fields = src.Fields ?? new AirTableRecordFields();
                })
                .ForMember(d => d.Text, expr => expr.MapFrom(src => src.Fields.Message))
                .ForMember(d => d.Title, expr => expr.MapFrom(src => src.Fields.Summary))
                .ForMember(d => d.ReceivedAt, expr => expr.MapFrom(src => src.Fields.ReceivedAt))
                .ForMember(d => d.Id, expr => expr.MapFrom(src => src.Fields.Id));

            CreateMap<EnrichedLogRecord, AirTableRecordData>()
                .BeforeMap((logRecord, airTableRecord) =>
                {
                    airTableRecord.Fields = airTableRecord.Fields ?? new AirTableRecordFields();
                    //airTableRecord.CreatedTime = DateTime.UtcNow;
                    //airTableRecord.Id = Guid.NewGuid().ToString();

                })
                .ForPath(d => d.Fields.Message, expr => expr.MapFrom(src => src.Text))
                .ForPath(d => d.Fields.Summary, expr => expr.MapFrom(src => src.Title))
                .ForPath(d => d.Fields.ReceivedAt, expr => expr.MapFrom(src => src.ReceivedAt))
                .ForPath(d => d.Fields.Id, expr => expr.MapFrom(src => src.Id))
                .ForMember(d => d.CreatedTime, expr => expr.Ignore())
                .ForMember(d => d.Id, expr => expr.Ignore());
        }
    }
}
