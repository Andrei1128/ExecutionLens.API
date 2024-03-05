using Microsoft.Extensions.DependencyInjection;
using PostMortem.Application.Contracts.Application;
using PostMortem.Application.Implementations;

namespace PostMortem.Application;

public static partial class ServiceCollection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDiagramService,DiagramService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IMetricsService, MetricsService>();
        services.AddScoped<IPredictionService, PredictionService>();
    }
}
