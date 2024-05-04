using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Extensions;
using ExecutionLens.Domain.Enums;
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
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.Ids(i => i.Values(nodeId)),
                        sh => sh.Prefix(p => p.NodePath, nodeId)
                    )
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
                Time = timeDifference
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
                .Terms(nameof(ElasticTerm.classGroup), t => t
                    .Field(f => f.Class.Suffix(nameof(ElasticTerm.keyword)))
                    .Aggregations(aa => aa
                        .Terms(nameof(ElasticTerm.methodGroup), tt => tt
                            .Field(ff => ff.Method.Suffix(nameof(ElasticTerm.keyword)))
                        )
                    )
                )
            )
        );

        List<RequestCount> requestCounts = [];

        var groupByClass = response.Aggregations.Terms(nameof(ElasticTerm.classGroup));

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms(nameof(ElasticTerm.methodGroup));

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
                .Terms(nameof(ElasticTerm.classGroup), group => group
                    .Field(f => f.Class.Suffix(nameof(ElasticTerm.keyword)))
                    .Aggregations(subAgg => subAgg
                        .Terms(nameof(ElasticTerm.methodGroup), subGroup => subGroup
                            .Field(f => f.Method.Suffix(nameof(ElasticTerm.keyword)))
                        )
                    )
                )
            )
        );

        List<ExceptionCount> exceptionCounts = [];

        var groupByClass = response.Aggregations.Terms(nameof(ElasticTerm.classGroup));

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms(nameof(ElasticTerm.methodGroup));

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
                .Terms(nameof(ElasticTerm.classGroup), classAgg => classAgg
                    .Field(f => f.Class.Suffix(nameof(ElasticTerm.keyword)))
                    .Aggregations(classSubAgg => classSubAgg
                        .Terms(nameof(ElasticTerm.methodGroup), methodAgg => methodAgg
                            .Field(f => f.Method.Suffix(nameof(ElasticTerm.keyword)))
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

        var groupByClass = response.Aggregations.Terms(nameof(ElasticTerm.classGroup));

        foreach (var classGroup in groupByClass.Buckets)
        {
            var groupByMethod = classGroup.Terms(nameof(ElasticTerm.methodGroup));

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