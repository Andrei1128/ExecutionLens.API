using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController(ILogsService _searchService) : ControllerBase
{
    [HttpGet] //permite cautare de tip full text search
    [Route("SearchLogs")]
    public async Task<IActionResult> SearchLogs()
    {
        return Ok();
    }

    [HttpGet] //cautare pentru un anumit log pe baza de id
    [Route("GetLog/{logId}")]
    public async Task<IActionResult> GetLog(string logId)
    {
        return Ok();
    }

    [HttpGet]
    [Route("GetEndpoints")]
    public async Task<IActionResult> GetEndpoints()
    {
        return Ok();
    }
}
