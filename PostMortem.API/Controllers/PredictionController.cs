using Microsoft.AspNetCore.Mvc;

namespace PostMortem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate dintr-un anumit punct de pe harta(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetGeolocationPredictions()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate per endpoint(pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetRequestsCountPredictions()
        {
            return Ok();
        }
    }
}
