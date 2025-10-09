using CRMApp.Models;
using CRMApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class ContactInquiryController : Controller
    {
		private readonly IContactInquiryService _contactInquiryService;

		public ContactInquiryController(IContactInquiryService contactInquiryService)
        {
			_contactInquiryService = contactInquiryService;
		}

        public IActionResult Index()
        {
            var inquiries = _contactInquiryService.GetAllInquiries();
            return View(inquiries);
        }

        [HttpPost]
        public IActionResult SaveInquiry(ContactInquiry contactInquiry)
        {
            _contactInquiryService.SaveInquiry(contactInquiry);
            return RedirectToAction("Index", "Home");
        }
    }
}
