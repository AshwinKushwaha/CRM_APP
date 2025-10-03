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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
		{
			var users = _userService.GetAllUsers();
			var userViewModel = new UserViewModel()
			{
				Users = users
			};
			return View(userViewModel);
		}

		[HttpPost]
		public IActionResult Index(UserViewModel userViewModel)
		{
			var users = _userService.GetUsers(userViewModel.UserFilter, userViewModel.UserInput);
			var viewModel = new UserViewModel()
			{
				Users = users
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
