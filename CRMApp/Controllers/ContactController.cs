using CRMApp.Models;
using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddContact(CustomerViewModel viewModel)
        {
            if (viewModel.CustomerContact != null)
            {
                _contactService.CreateContact(viewModel.CustomerContact);
                return RedirectToAction("Details", "Customer", new { Id = viewModel.CustomerContact.CustomerId });
            }

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult DeleteContact(int id)
        {
            var CustId = _contactService.GetContact(id).CustomerId;
            if (!_contactService.DeleteContact(id))
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Customer", new { Id = CustId });

        }

    }
}
