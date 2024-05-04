using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using ExecutionLens.Domain.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Log;

[AllowAnonymous]
[HttpGet("Log/OverviewNode/{NodeId}")]
public class GetNodeOverviewEndpoint(ILogService _logService) : Endpoint<GetNodeOverviewRequest, NodeOverview?>
{
    public override async Task HandleAsync(GetNodeOverviewRequest request, CancellationToken ct)
    {
        NodeOverview? result = await _logService.GetNode(request.NodeId, request.NeedRoot);
        await SendAsync(result, cancellation: ct);
    }
}
