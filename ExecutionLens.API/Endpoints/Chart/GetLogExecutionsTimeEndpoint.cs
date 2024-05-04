using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Chart;

[AllowAnonymous]
[HttpGet("Chart/GetLogExecutionsTime/{Id}")]
public class GetLogExecutionsTimeEndpoint(IChartService _chartService) : Endpoint<IdRequest, List<ExecutionTime>>
{
    public override async Task HandleAsync(IdRequest request, CancellationToken ct)
    {
        List<ExecutionTime> executionTimes = await _chartService.GetLogExecutionsTime(request.Id);
        await SendAsync(executionTimes, cancellation: ct);
    }
}