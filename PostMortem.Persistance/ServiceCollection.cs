using Microsoft.Extensions.DependencyInjection;
using PostMortem.Application.Contracts.Persistance;
using PostMortem.Persistance.Repositories;

namespace PostMortem.Persistance;

public static partial class ServiceCollection
{
    public static void AddPersistanceServices(this IServiceCollection services)
    {
        services.AddScoped<ILogRepository, ElasticSearch>();
    }
}
