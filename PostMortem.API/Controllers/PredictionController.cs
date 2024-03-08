using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredictionController(IPredictionService _predictionService) : ControllerBase
{
    [HttpGet]
    [Route("GetGeolocationPredictions")]
    public async Task<IActionResult> GetGeolocationPredictions([FromQuery] string? endpoint, DateTime? startDate, DateTime? endDate)
    {
        return Ok();
    }

    [HttpGet]
    [Route("GetRequestsCountPredictions")]
    public async Task<IActionResult> GetRequestsCountPredictions([FromQuery] string? endpoint, DateTime? startDate, DateTime? endDate)
    {
        return Ok();
    }
}
