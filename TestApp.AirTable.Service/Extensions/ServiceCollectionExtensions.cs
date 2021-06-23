using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestApp.AirTable.Services;
using TestApp.AirTable.Services.Mappers;
using TestApp.LoggingProxy.Contracts.Configuration.AirTable;
using TestApp.LoggingProxy.Contracts.Models;
using TestApp.LoggingProxy.Services;

namespace TestApp.LoggingProxy.Contracts.Extensions.AirTable
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAirTableConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AirTableConfiguration>(op => configuration.GetSection("LoggingConsumer").Bind(op));
            return services;
        }

        public static IServiceCollection RegisterAirTableMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AirTableRecordDataProfile)));
            return services;
        }

        public static IServiceCollection RegisterAirTableServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IRequestBuilder>(
                    new RequestBuilder(
                        configuration.GetValue<string>("LoggingConsumer:LoggingConsumerBaseUrl"), 
                        configuration.GetValue<string>("LoggingConsumer:LoggingConsumerApiKey")));

            services
                .AddScoped<ILoggingProxyService<EnrichedLogRecord, EnrichedLogRecord>, AirTableProxyService>();

            return services;
        }
    }
}
