using ExecutionLens.Application.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Log;

[AllowAnonymous]
[HttpGet("Log/GetClassNames")]
public class GetClassNamesEndpoint(ILogService _logService) : EndpointWithoutRequest<List<string>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        List<string> controllerNames = await _logService.GetClassNames();
        await SendAsync(controllerNames, cancellation: ct);
    }
}
