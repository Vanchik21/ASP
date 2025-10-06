using Microsoft.AspNetCore.Mvc;

namespace WebFromCli.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        [HttpGet("who")]
        public IActionResult GetWho()
        {
            return Ok(new { Name = "Іван", Surname = "Захаров" });
        }

        [HttpGet("time")]
        public IActionResult GetTime()
        {
            return Ok(new { ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }
}
