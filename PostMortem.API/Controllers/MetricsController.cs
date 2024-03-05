using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMetricsService _metricsService) : ControllerBase
{
    [HttpPost] //creaza metrics-uri custom precum ex: nr de cumparari zilnice pentru un anumit produs 
    [Route(nameof(CreateMetric))]
    public async Task<IActionResult> CreateMetric()
    {
        return Ok();
    }

    [HttpGet] //preia metrics-urile create
    [Route(nameof(GetMetrics))]
    public async Task<IActionResult> GetMetrics()
    {
        return Ok();
    }
}
