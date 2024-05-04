using ExecutionLens.Application.Contracts;
using ExecutionLens.Application.Implementations;
using ExecutionLens.Domain.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace ExecutionLens.Application;

public static class ServiceCollection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IElasticClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<ElasticSettings>>();
            var settings = options.Value;

            var conSettings = new ConnectionSettings(new Uri(settings.Uri))
                           .DefaultIndex(settings.IndexName);

            return new ElasticClient(conSettings);
        });

        services.AddScoped<IChartService, ChartService>();
        services.AddScoped<IExportService, ExportService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}
