///OpenCatapultModelId:109
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleMvc.Admin.Models;
using SampleMvc.Core.Entities;
using SampleMvc.Core.Services;
using SampleMvc.Admin.Identity;

namespace SampleMvc.Admin.Controllers
{
    [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
    [Area("Admin")]
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
