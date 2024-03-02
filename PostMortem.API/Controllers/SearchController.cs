using Microsoft.AspNetCore.Mvc;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    [HttpGet] //permite cautare de tip full text search
    public async Task<IActionResult> GetLogs()
    {
        return Ok();
    }

    [HttpGet] //cautare pentru un anumit log pe baza de id
    public async Task<IActionResult> GetLog()
    {
        return Ok();
    }
}
