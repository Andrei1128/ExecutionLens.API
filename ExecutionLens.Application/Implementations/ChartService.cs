using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Extensions;
using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using ExecutionLens.Domain.Scripts;
using Nest;

namespace ExecutionLens.Application.Implementations;

internal class ChartService(IElasticClient _elasticClient) : IChartService
{
    public async Task<List<ExecutionTime>> GetLogExecutionsTime(string nodeId)
    {
        var result = await _elasticClient.GetAsync<MethodLog>(nodeId);

        if (!result.Found)
        {
            return [];
        }

        var node = result.Source;

        bool isRoot = node.NodePath is null;

        if (!isRoot)
        {
            string rootId = node.NodePath!.Split('/').First();

            var rootResult = await _elasticClient.GetAsync<MethodLog>(rootId);
            nodeId = rootResult.Id;
        }

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.Ids(i => i.Values(nodeId)),
                        sh => sh.Prefix(p => p.Field("nodePath.keyword").Value(nodeId)))
                )
            )
        );

        List<ExecutionTime> executionTimes = [];

        foreach (var hit in response.Hits)
        {
            var methodLog = hit.Source;
            var timeDifference = methodLog.ExitTime - methodLog.EntryTime;

            executionTimes.Add(new ExecutionTime
            {
                Class = methodLog.Class,
                Method = methodLog.Method,
                Time = timeDifference.TotalMilliseconds
            });
        }

        return executionTimes;
    }

    public async Task<List<RequestCount>> GetRequestsCount(GraphFilters filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .ApplyFilters(filters)
            .Aggregations(a => a
                .Terms("groupByClass", t => t
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(aa => aa
                        .Terms("groupByMethod", tt => tt
                            .Field(ff => ff.Method.Suffix("keyword"))
                        )
                    )
                )
            )
        );

        List<RequestCount> requestCounts = [];

        var groupByClass = response.Aggregations.Terms("groupByClass");

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms("groupByMethod");

            foreach (var methodGroup in groupByMethod.Buckets)
            {
                requestCounts.Add(
                    new RequestCount
                    {
                        Class = classGroup.Key,
                        Method = methodGroup.Key,
                        Count = methodGroup.DocCount
                    }
                );
            }
        }

        return requestCounts;
    }

    public async Task<List<ExceptionCount>> GetExceptionsCount(GraphFilters filters)
    {
        var withExceptionsFilter = new TermQuery
        {
            Field = Infer.Field<MethodLog>(f => f.HasException),
            Value = true
        };

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .ApplyFilters(filters, withExceptionsFilter)
            .Aggregations(a => a
                .Terms("groupByClass", group => group
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(subAgg => subAgg
                        .Terms("groupByMethod", subGroup => subGroup
                            .Field(f => f.Method.Suffix("keyword"))
                        )
                    )
                )
            )
        );

        List<ExceptionCount> exceptionCounts = [];

        var groupByClass = response.Aggregations.Terms("groupByClass");

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms("groupByMethod");

            foreach (var methodGroup in groupByMethod.Buckets)
            {
                exceptionCounts.Add(
                    new ExceptionCount
                    {
                        Class = classGroup.Key,
                        Method = methodGroup.Key,
                        Count = methodGroup.DocCount
                    }
                );
            }
        }

        return exceptionCounts;
    }

    public async Task<List<ExecutionTimes>> GetExecutionsTimes(GraphFilters filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .ApplyFilters(filters)
            .Aggregations(a => a
                .Terms("groupByClass", classAgg => classAgg
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(classSubAgg => classSubAgg
                        .Terms("groupByMethod", methodAgg => methodAgg
                            .Field(f => f.Method.Suffix("keyword"))
                            .Aggregations(methodSubAgg => methodSubAgg
                                .ScriptedMetric(ExecutionTimesScript.Name, sm => sm
                                    .InitScript(ExecutionTimesScript.Init)
                                    .MapScript(ExecutionTimesScript.Map)
                                    .CombineScript(ExecutionTimesScript.Combine)
                                    .ReduceScript(ExecutionTimesScript.Reduce)
                                )
                            )
                        )
                    )
                )
            )
        );

        List<ExecutionTimes> executionTimes = [];

        var groupByClass = response.Aggregations.Terms("groupByClass");

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms("groupByMethod");

            foreach (var methodGroup in groupByMethod.Buckets)
            {
                var stats = methodGroup.ScriptedMetric(ExecutionTimesScript.Name).Value<IDictionary<string, object>>();

                executionTimes.Add(
                    new ExecutionTimes
                    {
                        Class = classGroup.Key,
                        Method = methodGroup.Key,
                        Min = (double)stats["min"],
                        Max = (double)stats["max"],
                        Avg = (double)stats["avg"]
                    });
            }
        }

        return executionTimes;
    }
}