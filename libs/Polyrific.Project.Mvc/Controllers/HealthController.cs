using Microsoft.AspNetCore.Mvc;

namespace Polyrific.Project.Mvc.Controllers
{
    [Route("health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult CheckHealth()
        {
            return new OkResult();
        }
    }
}
