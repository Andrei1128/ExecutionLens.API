using ExecutionLens.API.DOMAIN.DTOs;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ExecutionLens.API.API.Endpoints;

[HttpPost("logs/GetMethodNames")]
[AllowAnonymous]
public class GetMethodNames(ILogRepository _logRepository) : Endpoint<string[], List<string>>
{
    public override async Task HandleAsync(string[] classList, CancellationToken ct)
    {
        try
        {   
            List<string> controllerNames = await _logRepository.GetMethodNames(classList);
            await SendAsync(controllerNames, cancellation: ct);
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            Console.WriteLine(err);
        }
    }
}
