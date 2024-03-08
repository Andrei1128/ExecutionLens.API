using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetricsController(IMetricsService _metricsService) : ControllerBase
{
    [HttpPost] 
    [Route("CreateMetric")]
    public async Task<IActionResult> CreateMetric()
    {
        return Ok();
    }

    [HttpGet] 
    [Route("GetMetrics")]
    public async Task<IActionResult> GetMetrics()
    {
        return Ok();
    }

    [HttpDelete] 
    [Route("DeleteMetric/{metricId}")]
    public async Task<IActionResult> DeleteMetric(string metricId)
    {
        return Ok();
    }
}
