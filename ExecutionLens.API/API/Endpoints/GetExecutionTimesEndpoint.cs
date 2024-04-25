using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("logs/GetExecutionTimes")]
[AllowAnonymous]
public class GetExecutionTimesEndpoint(ILogRepository _logRepository) : Endpoint<GraphFilters, List<ExecutionTimes>>
{
    public override async Task HandleAsync(GraphFilters filters, CancellationToken ct)
    {
        try
        {
            List<ExecutionTimes> exceptionCounts = await _logRepository.GetExecutionTimes(filters);
            await SendAsync(exceptionCounts, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
