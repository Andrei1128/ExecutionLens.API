using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Chart;

[AllowAnonymous]
[HttpPost("Chart/GetExceptionsCount")]
public class GetExceptionsCountEndpoint(IChartService _chartService) : Endpoint<GraphFilters, List<ExceptionCount>>
{
    public override async Task HandleAsync(GraphFilters filters, CancellationToken ct)
    {
        List<ExceptionCount> exceptionCounts = await _chartService.GetExceptionsCount(filters);
        await SendAsync(exceptionCounts, cancellation: ct);
    }
}
