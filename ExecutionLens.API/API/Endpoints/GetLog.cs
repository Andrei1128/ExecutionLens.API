using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.DOMAIN.Requests;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/{Id}")]
[AllowAnonymous]
public class GetLog(ILogRepository _logRepository) : Endpoint<GetLogRequest, MethodLog>
{
    public override async Task HandleAsync(GetLogRequest request, CancellationToken ct)
    {
        MethodLog? log = await _logRepository.GetLog(request.Id);
        await SendAsync(log, cancellation: ct);
    }
}
