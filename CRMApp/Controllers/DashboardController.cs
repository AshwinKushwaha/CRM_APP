using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class DashboardController : Controller
    {
		private readonly ICustomerService _customerService;
		private readonly IContactService _contactService;
		private readonly IActivityLogger _activityLogger;
        private readonly IContactInquiryService _contactInquiryService;

		public DashboardController(ICustomerService customerService, IContactService contactService, IActivityLogger activityLogger, IContactInquiryService contactInquiryService)
        {
			_customerService = customerService;
			_contactService = contactService;
            _activityLogger = activityLogger;
            _contactInquiryService = contactInquiryService;
		}

		

		public IActionResult Index()
        {
            

			ViewBag.CustomerCount = _customerService.GetCount();
            ViewBag.ContactCount = _contactService.GetCount();
            ViewBag.UserCount = _customerService.GetUserCount();
            
            
            if (User.IsInRole("admin"))
            {
                var allActivityLogs = _activityLogger.GetAllActivityLogs();
                var inquiries = _contactInquiryService.GetInquiries();
                var adminActivityViewModel = new ActivityLogViewModel(_contactService);
				

				if (allActivityLogs == null)
                {
                    adminActivityViewModel.activityLogs = new List<Models.ActivityLog>();
                    adminActivityViewModel.contactInquiries = new List<Models.ContactInquiry>();

				}
                else
                {
                    adminActivityViewModel.activityLogs = allActivityLogs;
                    adminActivityViewModel.contactInquiries = inquiries;
                }
				return View("AdminDashboard", adminActivityViewModel);
            }
            if (User.IsInRole("salesrep"))
            {
                var userActivityLogs = _activityLogger.GetActivityLogsByCurrentUser();
                var userActivityViewModel = new ActivityLogViewModel(_contactService);

				if (userActivityLogs == null)
                {
                    userActivityViewModel.activityLogs = new List<Models.ActivityLog>();
                }
                else
               
                
                return View("SalesDashboard", userActivityViewModel);
            }
            var activityLogs = _activityLogger.GetActivityLogsByCurrentUser();
			var activityViewModel = new ActivityLogViewModel(_contactService);
			if (activityLogs == null)
            {
                activityViewModel.activityLogs= new List<Models.ActivityLog>();
            }
            else
            {
                activityViewModel.activityLogs = activityLogs;
            }
            
            return View("SupportDashboard",activityViewModel);
        }
    }
}
