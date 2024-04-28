using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpDelete("search/{Id}")]
[AllowAnonymous]
public class DeleteSavedSearch(ILogRepository _logRepository) : Endpoint<DeleteSearch>
{
    public override async Task HandleAsync(DeleteSearch request, CancellationToken ct)
    {
         await _logRepository.DeleteSavedSearch(request.Id);
    }
}
