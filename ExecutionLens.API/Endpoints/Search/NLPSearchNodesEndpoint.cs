using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

[AllowAnonymous]
[HttpPost("Search/NLPSearchNodes")]
public class NLPSearchNodesEndpoint(ISearchService _searchService) : Endpoint<string, GetNodesResponse>
{
    public override async Task HandleAsync(string textQuery, CancellationToken ct)
    {
        GetNodesResponse result = await _searchService.NLPSearch(textQuery);
        await SendAsync(result, cancellation: ct);
    }
}
