using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PostMortem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagramsController : ControllerBase
    {
        [HttpGet]
        [Route("GetSequenceDiagramData/{logId}")]
        public async Task<IActionResult> GetSequenceDiagramData(string logId)
        {
            return Ok();
        }
    }
}
