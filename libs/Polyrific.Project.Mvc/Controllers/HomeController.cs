using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Polyrific.Project.Mvc.Controllers
{
    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
