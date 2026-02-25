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
        private readonly IWebsiteVisitService _websiteVisitService;

        public DashboardController(ICustomerService customerService, IContactService contactService, IActivityLogger activityLogger, IContactInquiryService contactInquiryService, IWebsiteVisitService websiteVisitService)
        {
            _customerService = customerService;
            _contactService = contactService;
            _activityLogger = activityLogger;
            _contactInquiryService = contactInquiryService;
            _websiteVisitService = websiteVisitService;
        }



        public IActionResult Index(int pageIndex = 1)
        {
            int pageSize = 10;


            ViewBag.CustomerCount = _customerService.GetCustomerCount();
            ViewBag.ContactCount = _contactService.GetContactCount();
            ViewBag.UserCount = _customerService.GetUserCount();
            
            // Add visit statistics for admin
            if (User.IsInRole("admin"))
            {
                ViewBag.VisitsToday = _websiteVisitService.GetTotalVisitsToday();
                ViewBag.VisitsThisWeek = _websiteVisitService.GetTotalVisitsThisWeek();
                ViewBag.VisitsThisMonth = _websiteVisitService.GetTotalVisitsThisMonth();
                ViewBag.DailyVisitStats = System.Text.Json.JsonSerializer.Serialize(_websiteVisitService.GetDailyVisitStats(30));
            }


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
