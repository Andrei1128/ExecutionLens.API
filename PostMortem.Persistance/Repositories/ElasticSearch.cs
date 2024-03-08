using Microsoft.Extensions.Options;
using Nest;
using PostMortem.Application.Contracts.Persistance;
using PostMortem.Domain;
using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;
using PostMortem.Domain.Models;
using PostMortem.Persistance.Extensions;

namespace PostMortem.Persistance.Repositories;

internal class ElasticSearch : ILogRepository
{
    private readonly IElasticClient _elasticClient;
    private readonly string _index;

    public ElasticSearch(IOptions<AppSettings> options)
    {
        var appSettings = options.Value;
        _index = appSettings.Elastic.IndexName;

        var connectionSettings = new ConnectionSettings(new Uri(appSettings.Elastic.Uri))
            .DefaultIndex(_index)
            .ThrowExceptions();

        _elasticClient = new ElasticClient(connectionSettings);
    }

    public async Task<MethodLog?> GetLog(string logId) => (await _elasticClient.GetAsync<MethodLog>(logId)).Source;

    public async Task<IEnumerable<EndpointCallsCount>> GetEndpointsCallsCount(Filters filters)
    {
        throw new NotImplementedException();
        //var response = await _elasticClient.SearchAsync<MethodLog>(s => s
        //    .Size(0)
        //    .BetweenDates(filters.StartDate!.Value, filters.EndDate!.Value)
        //    .WithControllerIfExists(filters.Cont)
        //    .WithEndpointIfExists(filters.EndpointName)
        //    .Aggregations(a => a
        //        .Terms("class_agg", t => t
        //            .Field(f => f.Entry.Class)
        //            .Aggregations(aa => aa
        //                .Terms("method_agg", tt => tt
        //                    .Field(ff => ff.Entry.Method)
        //                )
        //            )
        //        )
        //    )
        //);

        //var classAgg = response.Aggregations.Terms("class_agg");

        //return classAgg.Buckets.SelectMany(
        //    classBucket =>
        //    classBucket.Terms("method_agg").Buckets.Select(
        //        methodBucket => new EndpointCallsCount
        //        {
        //            Endpoint = $"{classBucket.Key}:{methodBucket.Key}",
        //            Count = methodBucket.DocCount
        //        })
        //);
    }

    public Task<IEnumerable<EndpointGroupExecutionTime>> GetEndpointsExecutionsTime(Filters filters)
    {
        throw new NotImplementedException();
    }
}
