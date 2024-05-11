using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

[AllowAnonymous]
[HttpPost("Search/NLPSearchNodes")]
public class NLPSearchNodesEndpoint(ISearchService _searchService) : Endpoint<NLPSearchRequest, NLPSearchResponse>
{
    public override async Task HandleAsync(NLPSearchRequest request, CancellationToken ct)
    {
        NLPSearchResponse result = await _searchService.NLPSearch(request.TextQuery);
        await SendAsync(result, cancellation: ct);
    }
}
