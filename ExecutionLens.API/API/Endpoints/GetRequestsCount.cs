using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/GetRequestsCount")]
[AllowAnonymous]
public class GetRequestsCount(ILogRepository _logRepository) : EndpointWithoutRequest<List<RequestCount>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {   
            List<RequestCount> exceptionCounts = await _logRepository.GetRequestsCount();
            await SendAsync(exceptionCounts, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
