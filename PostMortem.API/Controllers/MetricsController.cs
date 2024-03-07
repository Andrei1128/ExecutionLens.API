using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMetricsService _metricsService) : ControllerBase
{
    [HttpPost] //creaza metrics-uri custom precum ex: nr de cumparari zilnice pentru un anumit produs 
    [Route("CreateMetric")]
    public async Task<IActionResult> CreateMetric()
    {
        return Ok();
    }

    [HttpGet] //preia metrics-urile create
    [Route("GetMetrics")]
    public async Task<IActionResult> GetMetrics()
    {
        return Ok();
    }

    [HttpDelete] //preia metrics-urile create
    [Route("DeleteMetric/{metricId}")]
    public async Task<IActionResult> DeleteMetric(string metricId)
    {
        return Ok();
    }
}
