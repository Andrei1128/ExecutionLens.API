using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("search")]
[AllowAnonymous]
public class SaveSearchEndpoint(ILogRepository _logRepository) : Endpoint<SavedSearch>
{
    public override async Task HandleAsync(SavedSearch request, CancellationToken ct)
    {
         await _logRepository.SaveSearch(request);
    }
}
