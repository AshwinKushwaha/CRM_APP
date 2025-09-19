using CRMApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationUserIdentityContext context;

        public CustomerController(ApplicationUserIdentityContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var cust = context.Customers.ToList();
            return View(cust);
        }
    }
}
