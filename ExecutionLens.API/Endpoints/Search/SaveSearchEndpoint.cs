using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Search;

[AllowAnonymous]
[HttpPost("Search/Save")]
public class SaveSearchEndpoint(ISearchService _searchService) : Endpoint<SavedSearch>
{
    public override async Task HandleAsync(SavedSearch request, CancellationToken ct)
    {
        await _searchService.SaveSearch(request);
    }
}
