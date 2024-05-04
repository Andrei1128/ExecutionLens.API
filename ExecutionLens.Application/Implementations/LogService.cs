using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Enums;
using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using ExecutionLens.Domain.Utilities;
using Microsoft.Extensions.Options;
using Nest;

namespace ExecutionLens.Application.Implementations;

internal class LogService(IElasticClient _elasticClient, IOptions<QuerySettings> _querySettings) : ILogService
{
    public async Task<MethodLog?> GetLog(string logId)
    {
        var result = await _elasticClient.GetAsync<MethodLog>(logId);

        if (!result.Found)
        {
            return null;
        }

        var node = result.Source;

        bool isRoot = node.NodePath is null;

        List<string> definitions = [];

        if (isRoot)
        {
            await AddInteractions(node, logId);
        }
        else
        {
            string rootId = node.NodePath!.Split('/').First();

            var rootResult = await _elasticClient.GetAsync<MethodLog>(rootId);
            node = rootResult.Source;

            await AddInteractions(node, rootResult.Id);
        }

        return node;
    }

    private async Task AddInteractions(MethodLog parent, string path)
    {
        var searchResponse = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Query(q => q
                .Term(t => t.Field(f => f.NodePath.Suffix("keyword")).Value(path))
            )
        );

        foreach (var hit in searchResponse.Hits)
        {
            MethodLog interactions = hit.Source;

            parent.Interactions.Add(interactions);

            await AddInteractions(interactions, $"{path}/{hit.Id}");
        }
    }

    public async Task<List<string>> GetClassNames()
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .Aggregations(a => a
                .Terms("class", terms => terms
                    .Field(f => f.Class.Suffix("keyword"))
                )
            )
        );

        List<string> classNames = [];

        var uniqueClassesAgg = response.Aggregations.Terms("class");

        foreach (var bucket in uniqueClassesAgg.Buckets)
        {
            classNames.Add(bucket.Key);
        }

        return classNames;
    }

    public async Task<List<string>> GetMethodNames(string[] classNames)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .Query(q => q
                .Terms(t => t
                    .Field(f => f.Class.Suffix("keyword"))
                    .Terms(classNames)
                )
            )
            .Aggregations(a => a
                .Terms("class", terms => terms
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(classAgg => classAgg
                        .Terms("method", methodAgg => methodAgg
                            .Field(f => f.Method.Suffix("keyword"))
                        )
                    )
                )
            )
        );

        List<string> methodNames = [];

        var filteredClassesAgg = response.Aggregations.Terms("class");

        foreach (var classBucket in filteredClassesAgg.Buckets)
        {
            var uniqueMethodsAgg = classBucket.Terms("method");

            foreach (var methodBucket in uniqueMethodsAgg.Buckets)
            {
                methodNames.Add(methodBucket.Key);
            }
        }

        return methodNames;
    }

    public async Task<MethodExceptionsResponse> GetMethodExceptions(MethodExceptionsRequest method)
    {
        int pageSize = _querySettings.Value.ExceptionsPageSize;

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Query(q => q
                .Bool(b => b
                    .Must(
                        bs => bs.Term(t => t.Field(f => f.Class.Suffix("keyword")).Value(method.Class)),
                        bs => bs.Term(t => t.Field(f => f.Method.Suffix("keyword")).Value(method.Name)),
                        bs => bs.Term(t => t.Field(f => f.HasException).Value(true))
                    )
                )
            )
            .Sort(st => st.Descending(f => f.ExitTime))
            .From(method.Page * pageSize)
            .Size(pageSize)
            .Source(src => src
                .Includes(i => i
                    .Fields(
                        f => f.ExitTime,
                        f => f.Output
                    )
                )
            )
        );

        var result = new MethodExceptionsResponse()
        {
            TotalEntries = response.Total
        };

        foreach (var hit in response.Hits)
        {
            var document = hit.Source;
            var documentId = hit.Id;

            result.Exceptions.Add(
                new NodeException
                {
                    NodeId = documentId,
                    OccuredAt = document.ExitTime,
                    Exception = document.Output
                });
        }

        return result;
    }

    public async Task<NodeOverview?> GetNode(string id, bool needRoot)
    {
        var result = await _elasticClient.GetAsync<MethodLog>(id);

        if (!result.Found)
        {
            return null;
        }

        MethodLog node = result.Source;

        bool isRoot = node.NodePath is null;

        if (needRoot && !isRoot)
        {
            string rootId = node.NodePath!.Split('/').First();

            var rootResult = await _elasticClient.GetAsync<MethodLog>(rootId);
            node = rootResult.Source;
        }

        return new NodeOverview()
        {
            Id = id,
            Class = node.Class,
            Method = node.Method,
            EntryTime = node.EntryTime,
            ExitTime = node.ExitTime,
            HasException = node.HasException,
            Duration = node.ExitTime - node.EntryTime
        };
    }
}
