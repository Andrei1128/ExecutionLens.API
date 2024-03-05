using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController(ISearchService _searchService) : ControllerBase
{
    [HttpGet] //permite cautare de tip full text search
    [Route(nameof(SearchLogs))]
    public async Task<IActionResult> SearchLogs()
    {
        return Ok();
    }

    [HttpGet] //cautare pentru un anumit log pe baza de id
    [Route(nameof(GetLog))]
    public async Task<IActionResult> GetLog()
    {
        return Ok();
    }
}
