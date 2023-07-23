using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Service
{
    [ApiController]
    [Route("road")]
    [Authorize("road")]
    public class RoadController : ControllerBase
    {
        private readonly ILogger<RoadController> _logger;

        public RoadController(ILogger<RoadController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new [] 
            {
                new { name= "A", score= 3, parent= (string?)null },
                new { name= "B", score= 5, parent= (string?)"A" },
                new { name= "C", score= 6, parent= (string?)"A" },
                new { name= "D", score= 4, parent= (string?)"C" },
                new { name= "E", score= 2, parent= (string?)"D" },
                new { name= "F", score= 2, parent= (string?)"A" }
            });
        }
    }
}