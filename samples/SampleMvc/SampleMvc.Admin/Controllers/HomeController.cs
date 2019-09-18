using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleMvc.Admin.Identity;

namespace TestProject.Areas.Admin.Controllers
{
    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
