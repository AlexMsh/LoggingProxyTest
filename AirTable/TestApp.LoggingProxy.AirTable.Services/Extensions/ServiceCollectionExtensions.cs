using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestApp.LoggingProxy.AirTable.Contracts.Configuration;
using TestApp.LoggingProxy.AirTable.Services.Mappers;
using TestApp.LoggingProxy.Contracts;
using TestApp.LoggingProxy.Contracts.Models;

namespace TestApp.LoggingProxy.AirTable.Services.Extensions
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
