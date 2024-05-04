using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Log;

[AllowAnonymous]
[HttpGet("Log/GetMethodExceptions")]
public class GetMethodExceptionsEndpoint(ILogService _logService) : Endpoint<MethodExceptionsRequest, MethodExceptionsResponse>
{
    public override async Task HandleAsync(MethodExceptionsRequest request, CancellationToken ct)
    {
        MethodExceptionsResponse exceptions = await _logService.GetMethodExceptions(request);
        await SendAsync(exceptions, cancellation: ct);
    }
}
