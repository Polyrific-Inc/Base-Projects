using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Polyrific.Project.Mvc.Areas.Home.Controllers
{
    [Area("Home")]
    //[Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
