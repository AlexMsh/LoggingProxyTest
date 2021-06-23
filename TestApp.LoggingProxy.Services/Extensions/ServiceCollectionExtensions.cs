using Microsoft.Extensions.DependencyInjection;
using TestApp.LoggingProxy.Contracts.Models;
using TestApp.LoggingProxy.Services;

namespace TestApp.LoggingProxy.Contracts.Extensions.AirTable
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterLogEnricher(this IServiceCollection services)
        {
            services.AddScoped<ILoggingProxyService<LogRecord, EnrichedLogRecord>, LoggingProxyEnricher>();
            services.AddSingleton<DateTimeProvider>();
            return services;
        }
    }
}
