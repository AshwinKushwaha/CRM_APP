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


        public IActionResult ArchiveEnquiry(int id)
        {
            var inquiry = _contactInquiryService.GetInquiry(id);
            inquiry.isArchived = true;
            _contactInquiryService.SaveInquiry(inquiry);
            return RedirectToAction("Index");
        }
    }
}
