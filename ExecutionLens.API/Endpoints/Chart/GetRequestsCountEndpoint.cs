using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Chart;

[AllowAnonymous]
[HttpPost("Chart/GetRequestsCount")]
public class GetRequestsCountEndpoint(IChartService _chartService) : Endpoint<GraphFilters, List<RequestCount>>
{
    public override async Task HandleAsync(GraphFilters filters, CancellationToken ct)
    {
        List<RequestCount> exceptionCounts = await _chartService.GetRequestsCount(filters);
        await SendAsync(exceptionCounts, cancellation: ct);
    }
}
