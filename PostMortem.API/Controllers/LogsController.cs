using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;
using PostMortem.Application.Contracts.Persistance;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController(ILogRepository _logRepository) : ControllerBase
{
    [HttpGet] 
    [Route("SearchLogs")]
    public async Task<IActionResult> SearchLogs()
    {
        return Ok();
    }

    [HttpGet] 
    [Route("{logId}")]
    public async Task<IActionResult> GetLog(string logId)
    {
        return Ok(await _logRepository.GetLog(logId));
    }

    [HttpGet]
    [Route("GetEndpoints")]
    public async Task<IActionResult> GetEndpoints()
    {
        return Ok();
    }

    [HttpGet]
    [Route("GetControllers")]
    public async Task<IActionResult> GetControllers()
    {
        return Ok();
    }
}
