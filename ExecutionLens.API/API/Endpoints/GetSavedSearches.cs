using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("search")]
[AllowAnonymous]
public class GetSavedSearches(ILogRepository _logRepository) : EndpointWithoutRequest<IEnumerable<SavedSearch>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        IEnumerable<SavedSearch> searches = await _logRepository.GetSavedSearches();
        await SendAsync(searches, cancellation: ct);
    }
}
