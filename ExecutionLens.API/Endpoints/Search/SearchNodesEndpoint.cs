using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Search;

[AllowAnonymous]
[HttpPost("Search/SearchNodes")]
public class SearchNodesEndpoint(ISearchService _searchService) : Endpoint<SearchFilter, GetNodesResponse>
{
    public override async Task HandleAsync(SearchFilter filters, CancellationToken ct)
    {
        GetNodesResponse result = await _searchService.Search(filters);
        await SendAsync(result, cancellation: ct);
    }
}
