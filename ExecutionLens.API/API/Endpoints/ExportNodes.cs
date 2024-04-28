using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.DOMAIN.Extensions;
using ExecutionLens.API.DOMAIN.Models;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("logs/Export")]
[AllowAnonymous]
public class ExportNodes(ILogRepository _logRepository) : Endpoint<SearchFilter>
{
    public override async Task HandleAsync(SearchFilter filters, CancellationToken ct)
    {
        try
        {
            IEnumerable<MethodLog> data = await _logRepository.Export(filters);
            string serializedData = JsonConvert.SerializeObject(data);
            Stream stream = serializedData.GetStream();

            await SendStreamAsync(
                stream: stream,
                fileName: $"export_{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}.json",
                fileLengthBytes: stream.Length,
                contentType: "application/octet-stream");

            return;
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
