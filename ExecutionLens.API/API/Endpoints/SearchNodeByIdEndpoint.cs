using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/Search/{id}")]
[AllowAnonymous]
public class SearchNodeByIdEndpoint(ILogRepository _logRepository) : Endpoint<SearchNodeByIdRequest, NodeOverview?>
{
    public override async Task HandleAsync(SearchNodeByIdRequest request, CancellationToken ct)
    {
        try
        {
            NodeOverview? result = await _logRepository.GetNode(request.Id);
            await SendAsync(result, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
