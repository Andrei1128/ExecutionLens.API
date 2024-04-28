using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("logs/Search")]
[AllowAnonymous]
public class SearchNodesEndpoints(ILogRepository _logRepository) : Endpoint<SearchFilter, GetNodesResponse>
{
    public override async Task HandleAsync(SearchFilter filters, CancellationToken ct)
    {
        try
        {
            GetNodesResponse result = await _logRepository.Search(filters);
            await SendAsync(result, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
