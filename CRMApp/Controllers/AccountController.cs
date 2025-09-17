using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRMApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(String Email, String Password)
        {
            if (Email == "ashwin@gmail.com" && Password == "1234")
            {
                var claims = new List<Claim>

                {
                    new Claim(ClaimTypes.Name, Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity),
                                authProperties);
            }
            //HttpContext.Session.SetString("username",Email);
            return RedirectToAction("AdminPage");
        }


        [Authorize]
        public IActionResult AdminPage()
        {
            ViewBag.Username = HttpContext.Session.GetString("username");
            return View("AdminPage");
        }

        [Authorize]
        public IActionResult Register()
        {
            ViewBag.Username = HttpContext.Session.GetString("username");
            return View();
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
