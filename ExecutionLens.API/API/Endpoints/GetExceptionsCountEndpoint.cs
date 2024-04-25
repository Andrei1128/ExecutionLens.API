using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("logs/GetExceptionsCount")]
[AllowAnonymous]
public class GetExceptionsCountEndpoint(ILogRepository _logRepository) : Endpoint<GraphFilters,List<ExceptionCount>>
{
    public override async Task HandleAsync(GraphFilters filters,CancellationToken ct)
    {
        try
        {
            List<ExceptionCount> exceptionCounts = await _logRepository.GetExceptionsCount(filters);
            await SendAsync(exceptionCounts, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
