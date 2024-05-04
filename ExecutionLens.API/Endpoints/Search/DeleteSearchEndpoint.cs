using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Search;

[AllowAnonymous]
[HttpDelete("Search/{Id}")]
public class DeleteSearchEndpoint(ISearchService _searchService) : Endpoint<IdRequest>
{
    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        await _searchService.DeleteSavedSearch(request.Id);
    }
}
