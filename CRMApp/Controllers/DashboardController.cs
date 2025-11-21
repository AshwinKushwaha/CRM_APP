using CRMApp.Models;
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



        public IActionResult Index(int pageIndex = 1)
        {
            int pageSize = 10;


            ViewBag.CustomerCount = _customerService.GetCustomerCount();
            ViewBag.ContactCount = _contactService.GetContactCount();
            ViewBag.UserCount = _customerService.GetUserCount();


            if (User.IsInRole("admin"))
            {
                var totalActivityLogs = _activityLogger.GetAllActivityLogs().Count;

                var activitylogs = _activityLogger.GetActivityLogs((int)pageIndex);
                var inquiries = _contactInquiryService.GetInquiries();
                var adminActivityViewModel = new ActivityLogViewModel(_contactService);


                if (activitylogs == null)
                {
                    adminActivityViewModel.activityLogs = new PaginatedList<ActivityLog>(activitylogs, totalActivityLogs, pageIndex, pageSize);
                    adminActivityViewModel.contactInquiries = new List<Models.ContactInquiry>();

                }
                else
                {
                    adminActivityViewModel.activityLogs = new PaginatedList<ActivityLog>(activitylogs, totalActivityLogs, pageIndex, pageSize);
                    adminActivityViewModel.contactInquiries = inquiries;
                }
                return View("AdminDashboard", adminActivityViewModel);
            }
            if (User.IsInRole("salesrep"))
            {
                var userActivityLogs = _activityLogger.GetActivityLogsByCurrentUser().Count;
                var activitylogs = _activityLogger.GetActivityLogsByCurrentUser(pageIndex);
                var userActivityViewModel = new ActivityLogViewModel(_contactService);

                if (activitylogs == null)
                {
                    userActivityViewModel.activityLogs = new PaginatedList<ActivityLog>(activitylogs, userActivityLogs, pageIndex, pageSize);
                }
                else
                {
                    userActivityViewModel.activityLogs = new PaginatedList<ActivityLog>(activitylogs, userActivityLogs, pageIndex, pageSize);
                }


                return View("SalesDashboard", userActivityViewModel);
            }
            var totalactivityLogs = _activityLogger.GetActivityLogsByCurrentUser().Count();
            var activityLogs = _activityLogger.GetActivityLogsByCurrentUser(pageIndex);
            var activityViewModel = new ActivityLogViewModel(_contactService);
            if (activityLogs == null)
            {
                activityViewModel.activityLogs = new PaginatedList<ActivityLog>(activityLogs, totalactivityLogs, pageIndex, pageSize);
            }
            else
            {
                activityViewModel.activityLogs = new PaginatedList<ActivityLog>(activityLogs, totalactivityLogs, pageIndex, pageSize);
            }

            return View("SupportDashboard", activityViewModel);
        }
    }
}
