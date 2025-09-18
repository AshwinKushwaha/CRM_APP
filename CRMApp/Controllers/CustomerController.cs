using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
