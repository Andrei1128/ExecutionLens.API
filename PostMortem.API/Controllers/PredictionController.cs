using Microsoft.AspNetCore.Mvc;
using PostMortem.Application.Contracts.Application;

namespace PostMortem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PredictionController(IPredictionService _predictionService) : ControllerBase
{
    [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate dintr-un anumit punct de pe harta(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
    [Route(nameof(GetGeolocationPredictions))]
    public async Task<IActionResult> GetGeolocationPredictions()
    {
        return Ok();
    }

    [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate per endpoint(pt o anumita perioada sau oricare)
    [Route(nameof(GetRequestsCountPredictions))]
    public async Task<IActionResult> GetRequestsCountPredictions()
    {
        return Ok();
    }
}
