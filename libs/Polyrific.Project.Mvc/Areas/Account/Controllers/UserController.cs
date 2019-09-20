using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Entities;
using Polyrific.Project.Mvc.Areas.Account.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Project.Mvc.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _userService.GetUsers();
            var models = _mapper.Map<List<UserViewModel>>(data);
            return View(models);
        }

        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, UserViewModel model)
        {
            try
            {
                var entity = _mapper.Map<User>(model);
                await _userService.CreateUser(entity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _userService.GetUserById(id);
            var model = _mapper.Map<UserViewModel>(data);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            try
            {
                var entity = _mapper.Map<User>(model);
                await _userService.UpdateUser(entity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            ViewData["Id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, UserViewModel model)
        {
            try
            {
                await _userService.DeleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["Id"] = id;
                return View();
            }
        }
    }
}
