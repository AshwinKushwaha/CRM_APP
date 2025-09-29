using CRMApp.Areas.Identity.Data;
using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CRMApp.Controllers
{
    public class DashboardController : Controller
    {
		private readonly ICustomerService _customerService;
		private readonly IContactService _contactService;
		private readonly IActivityLogger _activityLogger;

		public DashboardController(ICustomerService customerService, IContactService contactService, IActivityLogger activityLogger)
        {
			_customerService = customerService;
			_contactService = contactService;
            _activityLogger = activityLogger;
		}
        public IActionResult Index()
        {
            

			ViewBag.CustomerCount = _customerService.GetCount();
            ViewBag.ContactCount = _contactService.GetCount();
            ViewBag.UserCount = _customerService.GetUserCount();
            
            
            if (User.IsInRole("admin"))
            {
				var allActivityLogs = _activityLogger.GetAllActivityLogs();
                //var userName = _activityLogger.GetUserName(activityLogs);

                var adminActivityViewModel = new ActivityLogViewModel(_contactService)
				{
					activityLogs = allActivityLogs
				};
				return View("AdminDashboard", adminActivityViewModel);
            }
            if (User.IsInRole("salesrep"))
            {
                var userActivityLogs = _activityLogger.GetActivityLogsByCurrentuser();
                var userActivityViewModel = new ActivityLogViewModel(_contactService)
                {
                    activityLogs = userActivityLogs
				};
                return View("SalesDashboard", userActivityViewModel);
            }
            var activityLogs = _activityLogger.GetActivityLogsByCurrentuser();
            var activityViewModel = new ActivityLogViewModel(_contactService)
            {
                activityLogs = activityLogs
            };
            return View("SupportDashboard",activityViewModel);
        }
    }
}
