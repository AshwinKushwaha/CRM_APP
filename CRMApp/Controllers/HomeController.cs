using CRMApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRMApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index","Dashboard");
            }
            if (User.IsInRole("salesrep"))
            {
                return View("SalesDashboard");
            }
            if (User.IsInRole("support"))
            {
                return View("SupportDashboard");
            }
            return View();
        }

        
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
