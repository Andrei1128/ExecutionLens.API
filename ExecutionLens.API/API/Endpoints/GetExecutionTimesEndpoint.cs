using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/GetExecutionTimes")]
[AllowAnonymous]
public class GetExecutionTimesEndpoint(ILogRepository _logRepository) : EndpointWithoutRequest<List<ExecutionTimes>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            List<ExecutionTimes> exceptionCounts = await _logRepository.GetExecutionTimes();
            await SendAsync(exceptionCounts, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
