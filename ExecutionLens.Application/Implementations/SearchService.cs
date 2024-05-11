using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Extensions;
using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using Nest;
using Newtonsoft.Json;

namespace ExecutionLens.Application.Implementations;

internal class SearchService(IElasticClient _elasticClient, IOpenAIService _openAIService) : ISearchService
{
    public async Task<NLPSearchResponse> NLPSearch(string textQuery)
    {
        string jsonFilters = await _openAIService.GetJsonFromTextQuery(textQuery);

        var filters = JsonConvert.DeserializeObject<SearchFilter>(jsonFilters)!;

        var result = new NLPSearchResponse
        {
            Filters = filters
        };

        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
           .ApplySearchFilters(filters, filters.Filters?.ToQueryContainer())
           .ApplySort(filters.OrderBy)
           .From(filters.PageNo * filters.PageSize)
           .Size(filters.PageSize)
       );

        var resultData = new List<NodeOverview>();

        foreach (var hit in response.Hits)
        {
            resultData.Add(new NodeOverview
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

        result.Result = new GetNodesResponse()
        {
            Nodes = resultData,
            TotalEntries = response.Total
        };

        return result;
    }

    public async Task<GetNodesResponse> Search(SearchFilter filters)
    {
        var response = await _elasticClient.SearchAsync<MethodLog>(s => s
            .ApplySearchFilters(filters, filters.Filters?.ToQueryContainer())
            .ApplySort(filters.OrderBy)
            .From(filters.PageNo * filters.PageSize)
            .Size(filters.PageSize)
        );

        var result = new List<NodeOverview>();

        foreach (var hit in response.Hits)
        {
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

    public async Task SaveSearch(SavedSearch search)
    {
        search.SavedAt = DateTime.Now;
        var response = await _elasticClient.IndexAsync(search, idx => idx.Index($"{_elasticClient.ConnectionSettings.DefaultIndex}_searches"));
    }

    public async Task<IEnumerable<SavedSearch>> GetSavedSearches()
    {
        var searchResponse = await _elasticClient.SearchAsync<SavedSearch>(s => s
            .Index($"{_elasticClient.ConnectionSettings.DefaultIndex}_searches")
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
        var deleteResponse = await _elasticClient.DeleteAsync<SavedSearch>(id, d => d.Index($"{_elasticClient.ConnectionSettings.DefaultIndex}_searches"));
    }
}
