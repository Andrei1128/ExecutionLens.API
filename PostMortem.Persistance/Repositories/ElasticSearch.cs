using Microsoft.Extensions.Options;
using Nest;
using PostMortem.Application.Contracts.Persistance;
using PostMortem.Domain;

namespace PostMortem.Persistance.Repositories;

internal class ElasticSearch : ILogRepository
{
    private readonly IElasticClient _elasticClient;

    public ElasticSearch(IOptions<AppSettings> options)
    {
        var appSettings = options.Value;
        var connectionSettings = new ConnectionSettings(new Uri(appSettings.Elastic.Uri));
        _elasticClient = new ElasticClient(connectionSettings);
    }
}
