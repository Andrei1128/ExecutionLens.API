using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Search;

[AllowAnonymous]
[HttpGet("Search")]
public class GetSearchesEndpoint(ISearchService _searchService) : EndpointWithoutRequest<IEnumerable<SavedSearch>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<SavedSearch> searches = await _searchService.GetSavedSearches();
        await SendAsync(searches, cancellation: ct);
    }
}
