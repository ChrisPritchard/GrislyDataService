
using Microsoft.AspNetCore.Mvc;

namespace GrislyDataService.Controllers
{
    [Route("api/Entries")]
    public class EntryController : Controller
    {
        [HttpGet("")]
        public IActionResult Get([FromQuery]string entryName)
        {
            return Ok("Hello World");
        }
    }
}