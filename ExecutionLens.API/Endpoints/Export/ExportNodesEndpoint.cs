using ExecutionLens.Application.Contracts;
using ExecutionLens.Domain.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.Endpoints.Export;

[AllowAnonymous]
[HttpPost("Export/Nodes")]
public class ExportNodesEndpoint(IExportService _exportService) : Endpoint<SearchFilter>
{
    public override async Task HandleAsync(SearchFilter filters, CancellationToken ct)
    {
        Stream data = await _exportService.ExportNodes(filters);

        await SendStreamAsync(
            stream: data,
            fileName: $"export_{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}.json",
            fileLengthBytes: data.Length,
            contentType: "application/octet-stream",
            cancellation: ct);
    }
}
