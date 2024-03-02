﻿using Microsoft.AspNetCore.Mvc;

namespace PostMortem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagramController : ControllerBase
    {
        [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine metodele dintr-un log si timpii de executie
        public async Task<IActionResult> GetMethodsExecutionTime()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine  metodele si timpii de executie pentru toate logurile disponibile(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetExecutionsTimeOverview()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine  endpointurile si timpii de executie pentru toate logurile disponibile(pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetRequestsExecutionTimeOverview()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame de secvente pentru un anumit log
        public async Task<IActionResult> GetSequenceDiagramData()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru crearea unei diagrame ce contine metodele si exceptiile ce au fost aruncate pentru toate logurile disponibile(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetExceptionsDataOverview()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi dintr-un anumit punct de pe harta(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetRequestsGeolocation()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi per endpoint(pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetRequestsCount()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate dintr-un anumit punct de pe harta(pt orice endpoint sau unul anume, pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetAIGeolocationPredictions()
        {
            return Ok();
        }

        [HttpGet] //ofera informatii pentru creare unei diagrame ce contine nr de requesturi predictionate per endpoint(pt o anumita perioada sau oricare)
        public async Task<IActionResult> GetAIRequestsCountPredictions()
        {
            return Ok();
        }
    }
}
