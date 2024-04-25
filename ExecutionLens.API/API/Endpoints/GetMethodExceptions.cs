using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/GetMethodExceptions")]
[AllowAnonymous]
public class GetMethodExceptions(ILogRepository _logRepository) : Endpoint<MethodDTO, MethodExceptionsResponse>
{
    public override async Task HandleAsync(MethodDTO request, CancellationToken ct)
    {
        try
        {
            MethodExceptionsResponse exceptions = await _logRepository.GetMethodExceptions(request);
            await SendAsync(exceptions, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
