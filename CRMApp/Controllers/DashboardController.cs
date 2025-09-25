using CRMApp.Areas.Identity.Data;
using CRMApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class DashboardController : Controller
    {
		private readonly ICustomerService _customerService;
		private readonly IContactService _contactService;

		public DashboardController(ICustomerService customerService, IContactService contactService)
        {
			_customerService = customerService;
			_contactService = contactService;
		}
        public IActionResult Index()
        {
            

			ViewBag.CustomerCount = _customerService.GetCount();
            ViewBag.ContactCount = _contactService.GetCount();
            ViewBag.UserCount = _customerService.GetUserCount();
            if (User.IsInRole("admin"))
            {
                return View("AdminDashboard");
            }
            if (User.IsInRole("salesrep"))
            {
                return View("SalesDashboard");
            }
            return View("SupportDashboard");
        }
    }
}
