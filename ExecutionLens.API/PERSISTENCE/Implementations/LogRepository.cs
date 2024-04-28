using ExecutionLens.API.DOMAIN.Common;
using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.DOMAIN.Utilities;
using ExecutionLens.API.PERSISTENCE.Contracts;
using ExecutionLens.API.PERSISTENCE.Extensions;
using Microsoft.Extensions.Options;
using Nest;
using System;

namespace PostMortem.Persistance.Repositories;

internal class LogRepository : ILogRepository
{
    private readonly IElasticClient _elasticClient;
    private readonly string _index;
    private readonly QuerySettings _querySettings;

    public LogRepository(IOptions<ElasticSettings> elasticSettings, IOptions<QuerySettings> querySettings)
    {
        _querySettings = querySettings.Value;

        ElasticSettings _elasticSettings = elasticSettings.Value;

        _index = _elasticSettings.IndexName;

        var connectionSettings = new ConnectionSettings(new Uri(_elasticSettings.Uri))
            .DefaultIndex(_index)
            .ThrowExceptions();

        _elasticClient = new ElasticClient(connectionSettings);
    }

    public async Task<IEnumerable<MethodLog>> Export(SearchFilter filters)
    {
        var sortOrder = filters.OrderBy switch
        {
            "Date Ascending" => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Ascending },
            "Date Descending" => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Descending },
            "Score Ascending" => new FieldSort { Field = "_score", Order = SortOrder.Ascending },
            "Score Descending" => new FieldSort { Field = "_score", Order = SortOrder.Descending },
            _ => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Descending }
        };

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .ApplySearchFilters(filters)
            .Size(10_000)
            .Sort(ss => ss.Field(f => sortOrder))
        );

        Console.WriteLine(response.Documents.Count);

        return response.Documents;
    }

    private QueryContainer BuildQuery(List<AdvancedFilter>? filters)
    {
        var query = new QueryContainer();

        if (filters is null)
            return query;

        var filterGroups = filters.GroupBy(g => g.Target);

        foreach (var group in filterGroups)
        {
            Field target = GetField(group.Key.ToLower());
                
            var must = new QueryContainer();
            var mustNot = new QueryContainer();

            foreach (var filter in group)
            {
                if (filter.Operation.Equals("is", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= new TermQuery { Field = $"{target}.keyword", Value = filter.Value };
                }
                else if (filter.Operation.Equals("contains", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= new MatchQuery { Field = target, Query = filter.Value}; 
                }
                else if (filter.Operation.Equals("like", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= new WildcardQuery { Field = target, Wildcard = $"*{filter.Value}*" };
                }
                else if (filter.Operation.Equals("not", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= !new TermQuery { Field = $"{target}.keyword", Value = filter.Value };
                }
                else if (filter.Operation.Equals("not contains", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= !new MatchQuery { Field = target, Query = filter.Value};
                }
                else if (filter.Operation.Equals("not like", StringComparison.CurrentCultureIgnoreCase))
                {
                    must |= !new WildcardQuery { Field = target, Wildcard = $"*{filter.Value}*" };
                }
            }

            query &= new BoolQuery
            {
                Should = new List<QueryContainer> { must },
            };
        }

        return query;
    }

    private string GetField(string name)
    {
        return name switch
        {
            "input" => "input.value",
            "output" => "output.value",
            "information" => "informations.message",
            _ => throw new ArgumentException("Unsupported operation"),
        };
    }

    public async Task<GetNodesResponse> Search(SearchFilter filters)
    {
        var sortOrder = filters.OrderBy switch
        {
            "Date Ascending" => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Ascending },
            "Date Descending" => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Descending },
            "Score Ascending" => new FieldSort { Field = "_score", Order = SortOrder.Ascending },
            "Score Descending" => new FieldSort { Field = "_score", Order = SortOrder.Descending },
            _ => new FieldSort { Field = Infer.Field<MethodLog>(f => f.EntryTime), Order = SortOrder.Descending }
        };

        var advancedFilters = BuildQuery(filters.Filters);

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .ApplySearchFilters(filters, advancedFilters)
            .Sort(ss => ss.Field(f => sortOrder))
            .From(filters.PageNo * filters.PageSize)
            .Size(filters.PageSize)
        );

        var result = new List<NodeOverview>();

        foreach (var hit in response.Hits)
        {
            Console.WriteLine(hit.Score);
            result.Add(new NodeOverview
            {
                Id = hit.Id,
                Class = hit.Source.Class,
                Method = hit.Source.Method,
                EntryTime = hit.Source.EntryTime,
                ExitTime = hit.Source.ExitTime,
                HasException = hit.Source.HasException,
                Duration = hit.Source.ExitTime - hit.Source.EntryTime
            });
        }

        return new GetNodesResponse()
        {
            Nodes = result,
            TotalEntries = response.Total
        };
    }

    public async Task<NodeOverview?> GetNode(string id)
    {
        var result = (await _elasticClient.GetAsync<MethodLog>(id, idx => idx.Index(_index))).Source;

        if (result is null)
        {
            return null;
        }

        return new NodeOverview()
        {
            Id = id,
            Class = result.Class,
            Method = result.Method,
            EntryTime = result.EntryTime,
            ExitTime = result.ExitTime,
            HasException = result.HasException,
            Duration = result.ExitTime - result.EntryTime
        };
    }

    public async Task SaveSearch(SavedSearch search)
    {
        search.SavedAt = DateTime.Now;
        var response = await _elasticClient.IndexAsync(search, idx => idx.Index($"{_index}_searches"));
        Console.Write(response);
    }

    public async Task<IEnumerable<SavedSearch>> GetSavedSearches()
    {
        var searchResponse = await _elasticClient.SearchAsync<SavedSearch>(s => s
            .Index($"{_index}_searches")
            .Size(1000)
            .Sort(sort => sort
                .Descending(ss => ss.SavedAt))
        );

        var results = searchResponse.Hits.Select(hit =>
        {
            var savedSearch = hit.Source;
            savedSearch.Id = hit.Id;
            return savedSearch;
        });

        return results;
    }

    public async Task DeleteSavedSearch(string id)
    {
        var deleteResponse = await _elasticClient.DeleteAsync<SavedSearch>(id, d => d.Index($"{_index}_searches"));
    }

    private IEnumerable<MethodLog> GetAllNodes(string rootId)
    {
        var response = _elasticClient.Search<MethodLog>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.Ids(i => i.Values(rootId)),
                        sh => sh.Prefix(p => p.NodePath, rootId)
                    )
                )
            )
        );

        return response.Documents;
    }

    public async Task<List<ExceptionCount>> GetExceptionsCount(GraphFilters filters)
    {
        var initialFilter = new TermQuery { Field = Infer.Field<MethodLog>(f => f.HasException), Value = true };

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .ApplyFilters(filters, initialFilter)
            .Aggregations(a => a
                .Terms("group_by_class", group => group
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(subAgg => subAgg
                        .Terms("group_by_method", subGroup => subGroup
                            .Field(f => f.Method.Suffix("keyword"))
                        )
                    )
                )
            )
        );

        List<ExceptionCount> exceptionCounts = new List<ExceptionCount>();

        // Processing the results
        if (response.IsValid)
        {
            var groupByClass = response.Aggregations.Terms("group_by_class");
            foreach (var classGroup in groupByClass.Buckets)
            {
                Console.WriteLine($"Class: {classGroup.Key} - Count: {classGroup.DocCount}");
                var groupByMethod = classGroup.Terms("group_by_method");
                foreach (var methodGroup in groupByMethod.Buckets)
                {
                    exceptionCounts.Add(new ExceptionCount { Class = classGroup.Key, Method = methodGroup.Key, Count = methodGroup.DocCount });
                    Console.WriteLine($"  Method: {methodGroup.Key} - Count: {methodGroup.DocCount}");
                }
            }
        }
        else
        {
            Console.WriteLine("Query failed: " + response.DebugInformation);
        }


        return exceptionCounts;
    }

    public async Task<List<ExecutionTimes>> GetExecutionTimes(GraphFilters filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
    .Size(0)  // No documents are needed in the response, only aggregations
    .ApplyFilters(filters)
    .Aggregations(a => a
        .Terms("group_by_class", classAgg => classAgg
            .Field(f => f.Class.Suffix("keyword"))
            .Aggregations(classSubAgg => classSubAgg
                .Terms("group_by_method", methodAgg => methodAgg
                    .Field(f => f.Method.Suffix("keyword"))
                    .Aggregations(methodSubAgg => methodSubAgg
                        .ScriptedMetric("execution_time_stats", sm => sm
                            .InitScript("state.durations = []")  // Initialize state to hold durations
                            .MapScript("state.durations.add(doc['exitTime'].value.getMillis() - doc['entryTime'].value.getMillis());")  // Map script to calculate and collect durations
                                                                                                                                        //MapScript("state.durations.add(doc['ExitTime'].value.getMillis() - doc['EntryTime'].value.getMillis());")  // Map script to calculate and 
                            .CombineScript("double min = Double.POSITIVE_INFINITY; double max = Double.NEGATIVE_INFINITY; double sum = 0;" +
                                           "for (t in state.durations) {" +
                                           "  min = Math.min(min, t);" +
                                           "  max = Math.max(max, t);" +
                                           "  sum += t;" +
                                           "}" +
                                           "return [ 'min': min, 'max': max, 'avg': sum / state.durations.size(), 'sum': sum, 'count': state.durations.size() ];")  // Combine results within each shard
                            .ReduceScript("double min = Double.POSITIVE_INFINITY; double max = Double.NEGATIVE_INFINITY; double sum = 0; long count = 0;" +
                                          "for (a in states) {" +
                                          "  min = Math.min(min, a['min']);" +
                                          "  max = Math.max(max, a['max']);" +
                                          "  sum += a['sum'];" +
                                          "  count += a['count'];" +
                                          "}" +
                                          "double avg = count > 0 ? sum / count : 0;" +
                                          "return [ 'min': min, 'max': max, 'avg': avg ];")  // Reduce results across shards
                                                                                                         )
                                                                                                     )
                                                                                                 )
                                                                                             )
        )
    )
);

        List<ExecutionTimes> executionTimes = new List<ExecutionTimes>();

        // Handling response and errors
        if (response.IsValid)
        {
            var groupByClass = response.Aggregations.Terms("group_by_class");
            foreach (var classGroup in groupByClass.Buckets)
            {
                Console.WriteLine($"Class: {classGroup.Key}");
                var groupByMethod = classGroup.Terms("group_by_method");
                foreach (var methodGroup in groupByMethod.Buckets)
                {
                    var stats = methodGroup.ScriptedMetric("execution_time_stats").Value<IDictionary<string, object>>();

                    executionTimes.Add(new ExecutionTimes
                    {
                        Class = classGroup.Key,
                        Method = methodGroup.Key,
                        Min = (double)stats["min"],
                        Max = (double)stats["max"],
                        Avg = (double)stats["avg"]
                    });

                    Console.WriteLine($"  Method: {methodGroup.Key}");
                    Console.WriteLine($"    Min Execution Time: {stats["min"]} ms");
                    Console.WriteLine($"    Max Execution Time: {stats["max"]} ms");
                    Console.WriteLine($"    Avg Execution Time: {stats["avg"]} ms");
                }
            }
        }
        else
        {
            Console.WriteLine($"Query failed: {response.DebugInformation}");
        }

        return executionTimes;
    }

    public async Task<MethodLog?> GetLog(string logId)
    {
        var result = await _elasticClient.GetAsync<MethodLog>(logId, idx => idx.Index(_index));

        if (!result.Found)
        {
            return null;
        }

        var node = result.Source;

        bool isRoot = node.NodePath is null;

        if (isRoot)
        {
            await PopulateChildren(node, logId);
            return node;
        }
        else
        {
            string rootId = node.NodePath!.Split('/').First();

            var rootResult = await _elasticClient.GetAsync<MethodLog>(rootId, idx => idx.Index(_index));
            var root = rootResult.Source;

            await PopulateChildren(root, rootResult.Id);
            return root;
        }
    }

    private async Task PopulateChildren(MethodLog parent, string path)
    {
        var searchResponse = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Index(_index)
            .Query(q => q
                .Term(t => t.Field(f => f.NodePath.Suffix("keyword")).Value(path))
            )
        );


        if (searchResponse.Documents != null)
        {
            List<string> currentNodesIds = [];
            foreach (var document in searchResponse.Hits)
            {
                parent.Interactions.Add(document.Source);
                currentNodesIds.Add(document.Id);
            }

            for (int i = 0; i < parent.Interactions.Count; i++)
            {
                await PopulateChildren(parent.Interactions[i], $"{path}/{currentNodesIds[i]}");
            }
        }
    }

    public async Task<List<RequestCount>> GetRequestsCount(GraphFilters filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)
            .ApplyFilters(filters)
            .Aggregations(a => a
                .Terms("group_by_class", t => t
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(aa => aa
                        .Terms("group_by_method", tt => tt
                            .Field(ff => ff.Method.Suffix("keyword"))
                        )
                    )
                )
            )
            .Index(_index)
        );

        List<RequestCount> requestCounts = new List<RequestCount>();

        // To access the results
        if (response.IsValid && response.Aggregations.ContainsKey("group_by_class"))
        {
            var groupByClass = response.Aggregations.Terms("group_by_class");
            foreach (var classGroup in groupByClass.Buckets)
            {
                Console.WriteLine($"Class: {classGroup.Key}, Count: {classGroup.DocCount}");
                var groupByMethod = classGroup.Terms("group_by_method");
                foreach (var methodGroup in groupByMethod.Buckets)
                {
                    requestCounts.Add(new RequestCount { Class = classGroup.Key, Method = methodGroup.Key, Count = methodGroup.DocCount });
                    Console.WriteLine($"  Method: {methodGroup.Key}, Count: {methodGroup.DocCount}");
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve aggregations");
        }

        return requestCounts;
    }

    public async Task<List<string>> GetClassNames()
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)  // No documents are needed in the response, only aggregations
            .Aggregations(a => a
                .Terms("unique_classes", terms => terms
                    .Field(f => f.Class.Suffix("keyword"))  // Assumes there's a 'keyword' sub-field for exact matching
                                                            // Adjust this size to the expected number of unique classes, or use a larger value
                )
            )
        );

        var controllerNames = new List<string>();

        // Handling response and errors
        if (response.IsValid)
        {
            var uniqueClassesAgg = response.Aggregations.Terms("unique_classes");
            foreach (var bucket in uniqueClassesAgg.Buckets)
            {
                controllerNames.Add(bucket.Key);
                Console.WriteLine($"Class: {bucket.Key} - Count: {bucket.DocCount}");
            }
        }
        else
        {
            Console.WriteLine($"Query failed: {response.DebugInformation}");
        }

        return controllerNames;
    }

    public async Task<List<string>> GetMethodNames(string[] classList)
    {

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Size(0)  // No documents are needed in the response, only aggregations
            .Query(q => q
                .Terms(t => t
                    .Field(f => f.Class.Suffix("keyword"))
                    .Terms(classList)  // Filter to only include documents with these classes
                )
            )
            .Aggregations(a => a
                .Terms("filtered_classes", terms => terms
                    .Field(f => f.Class.Suffix("keyword"))
                    .Aggregations(classAgg => classAgg
                        .Terms("unique_methods", methodAgg => methodAgg
                            .Field(f => f.Method.Suffix("keyword"))
                        )
                    )
                )
            )
        );

        var methodNames = new List<string>();

        // Handling response and errors
        if (response.IsValid)
        {
            var filteredClassesAgg = response.Aggregations.Terms("filtered_classes");
            foreach (var classBucket in filteredClassesAgg.Buckets)
            {
                Console.WriteLine($"Class: {classBucket.Key}");
                var uniqueMethodsAgg = classBucket.Terms("unique_methods");
                foreach (var methodBucket in uniqueMethodsAgg.Buckets)
                {
                    methodNames.Add(methodBucket.Key);
                    Console.WriteLine($"  Method: {methodBucket.Key}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Query failed: {response.DebugInformation}");
        }
        return methodNames;
    }

    public async Task<MethodExceptionsResponse> GetMethodExceptions(MethodDTO method)
    {
        int pageSize = _querySettings.ExceptionsPageSize;

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

        if (response.IsValid)
        {
            foreach (var hit in response.Hits)
            {
                var document = hit.Source;
                var documentId = hit.Id;

                result.Exceptions.Add(new NodeExceptionDTO
                {
                    NodeId = documentId,
                    OccuredAt = document.ExitTime,
                    Exception = document.Output
                });
            }
        }
        else
        {
            Console.WriteLine($"Query failed: {response.DebugInformation}");
        }

        return result;
    }
}
