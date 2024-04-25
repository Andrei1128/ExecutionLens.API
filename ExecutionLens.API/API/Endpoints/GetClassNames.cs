using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpGet("logs/GetClassNames")]
[AllowAnonymous]
public class GetClassNames(ILogRepository _logRepository) : EndpointWithoutRequest<List<string>>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {   
            List<string> controllerNames = await _logRepository.GetClassNames();
            await SendAsync(controllerNames, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
