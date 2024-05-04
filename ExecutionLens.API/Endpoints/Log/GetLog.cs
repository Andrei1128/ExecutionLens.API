using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models;
using ExecutionLens.Domain.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Log;

[AllowAnonymous]
[HttpGet("Log/{Id}")]
public class GetLog(ILogService _logService) : Endpoint<IdRequest, MethodLog?>
{
    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        MethodLog? log = await _logService.GetLog(request.Id);
        await SendAsync(log, cancellation: ct);
    }
}
