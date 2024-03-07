using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;
using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagramController(IDiagramService _diagramService) : ControllerBase
{
    [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine metodele dintr-un log si timpii de executie
    [Route("GetMethodsExecutionTime/{logId}")]
    public async Task<ActionResult<List<MethodExecutionTime>>> GetMethodsExecutionTime(string logId)
    {
        List<MethodExecutionTime> execTimes = await _diagramService.GetMethodsExecutionTime(logId);

        return execTimes.Count > 0 ? Ok(execTimes) : NotFound();
    }

    [HttpGet] //ofera informatii pentru creare unei diagrame de secvente pentru un anumit log
    [Route("GetSequenceDiagramData/{logId}")]
    public async Task<IActionResult> GetSequenceDiagramData(string logId)
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine  metodele si timpii de executie pentru toate logurile disponibile(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
    [Route("GetExecutionsTimeOverview")]
    public async Task<IActionResult> GetExecutionsTimeOverview([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine  endpointurile si timpii de executie pentru toate logurile disponibile(pt o anumita perioada sau oricare)
    [Route("GetRequestsExecutionTimeOverview")]
    public async Task<IActionResult> GetRequestsExecutionTimeOverview([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine metodele si exceptiile ce au fost aruncate pentru toate logurile disponibile(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
    [Route("GetExceptionsDataOverview")]
    public async Task<IActionResult> GetExceptionsDataOverview([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi dintr-un anumit punct de pe harta(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
    [Route("GetRequestsGeolocation")]
    public async Task<IActionResult> GetRequestsGeolocation([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi per endpoint(pt o anumita perioada sau oricare)
    [Route("GetEndpointsCallsCount")]
    public async Task<ActionResult<IEnumerable<EndpointCallsCount>>> GetEndpointsCallsCount([FromQuery] Filters filters)
    {
        var result = await _diagramService.GetEndpointsCallsCount(filters.Validate());

        if(result.Any())
            return Ok(result);

        return NoContent();
    }
}
