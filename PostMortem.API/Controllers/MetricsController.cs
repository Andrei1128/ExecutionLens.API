using Microsoft.AspNetCore.Mvc;

namespace PostMortem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        [HttpPost] //creaza metrics-uri custom precum ex: nr de cumparari zilnice pentru un anumit produs 
        public async Task<IActionResult> CreateMetric()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMetrics() //preia metrics-urile create
        {
            return Ok();
        }
    }
}
