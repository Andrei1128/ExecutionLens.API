using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/GetExceptionsCount")]
[AllowAnonymous]
public class GetExceptionsCountEndpoint(ILogRepository _logRepository) : EndpointWithoutRequest<List<ExceptionCount>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            List<ExceptionCount> exceptionCounts = await _logRepository.GetExceptionsCount();
            await SendAsync(exceptionCounts, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
