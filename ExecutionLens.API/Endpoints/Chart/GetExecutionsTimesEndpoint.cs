using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Chart;

[AllowAnonymous]
[HttpPost("Chart/GetExecutionsTimes")]
public class GetExecutionsTimesEndpoint(IChartService _chartService) : Endpoint<GraphFilters, List<ExecutionTimes>>
{
    public override async Task HandleAsync(GraphFilters filters, CancellationToken ct)
    {
        List<ExecutionTimes> exceptionCounts = await _chartService.GetExecutionsTimes(filters);
        await SendAsync(exceptionCounts, cancellation: ct);
    }
}
