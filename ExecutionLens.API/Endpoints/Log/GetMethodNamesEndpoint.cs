using ExecutionLens.Application.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Log;

[AllowAnonymous]
[HttpPost("Log/GetMethodNames")]
public class GetMethodNamesEndpoint(ILogService _logService) : Endpoint<string[], List<string>>
{
    public override async Task HandleAsync(string[] classNames, CancellationToken ct)
    {
        List<string> controllerNames = await _logService.GetMethodNames(classNames);
        await SendAsync(controllerNames, cancellation: ct);
    }
}
