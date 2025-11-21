using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly int PageSize = 8;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            var users = _userService.GetUsers(pageIndex);
            var totalUsers = _userService.GetUserCount();
            var userViewModel = new UserViewModel()
            {
                Users = new PaginatedList<ApplicationUser>(users, totalUsers, pageIndex, PageSize)
            };
            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Index(UserViewModel userViewModel, int pageIndex)
        {
            var totalUsers = _userService.GetUsers(userViewModel.UserFilter, userViewModel.UserInput).Count();
            var users = _userService.GetUsers(userViewModel.UserFilter, userViewModel.UserInput, pageIndex);
            var viewModel = new UserViewModel()
            {
                Users = new PaginatedList<ApplicationUser>(users, totalUsers, pageIndex, PageSize)
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
