using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;
using PostMortem.Domain.Common;
using PostMortem.Domain.DTOs;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController(IDiagramService _diagramService) : ControllerBase
{
    [HttpGet]
    [Route("GetMethodsExecutionTime/{logId}")]
    public async Task<ActionResult<List<MethodExecutionTime>>> GetMethodsExecutionTime(string logId)
    {
        List<MethodExecutionTime> execTimes = await _diagramService.GetMethodsExecutionTime(logId);

        return execTimes.Count > 0 ? Ok(execTimes) : NoContent();
    }

    [HttpGet] 
    [Route("GetExecutionsTimeOverview")]
    public async Task<ActionResult<IEnumerable<EndpointGroupExecutionTime>>> GetExecutionsTimeOverview([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] 
    [Route("GetRequestsExecutionTimeOverview")]
    public async Task<ActionResult<IEnumerable<EndpointGroupExecutionTime>>> GetRequestsExecutionTimeOverview([FromQuery] Filters filters)
    {
        var result = await _diagramService.GetRequestsExecutionTimeOverview(filters.Validate());

        if (result.Any())
            return Ok(result);

        return NoContent();
    }

    [HttpGet]
    [Route("GetExceptionsDataOverview")]
    public async Task<ActionResult> GetExceptionsDataOverview([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] 
    [Route("GetRequestsGeolocation")]
    public async Task<ActionResult> GetRequestsGeolocation([FromQuery] Filters filters)
    {
        return Ok();
    }

    [HttpGet] 
    [Route("GetEndpointsCallsCount")]
    public async Task<ActionResult<IEnumerable<EndpointCallsCount>>> GetEndpointsCallsCount([FromQuery] Filters filters)
    {
        var result = await _diagramService.GetEndpointsCallsCount(filters.Validate());

        if(result.Any())
            return Ok(result);

        return NoContent();
    }
}
