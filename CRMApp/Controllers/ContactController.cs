using CRMApp.Models;
using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ICustomerService _customerService;
        private readonly int PageSize = 5;

        public ContactController(IContactService contactService, ICustomerService customerService)
        {
            _contactService = contactService;
            _customerService = customerService;
        }
        public IActionResult Index(int pageIndex = 1)
        {
            if (User.IsInRole("admin"))
            {
                var result = _contactService.GetAllContacts(pageIndex);
                var contacts = result.contacts;
                var totalContacts = result.contactCount;
                var viewModel = new ContactViewModel(_customerService)
                {
                    Contacts = new PaginatedList<CustomerContact>(contacts, totalContacts, pageIndex, PageSize)
                };
                return View(viewModel);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel contactViewModel, int pageIndex)
        {
            var totalContacts = _contactService.GetContacts(contactViewModel.ContactFilter, contactViewModel.ContactInput, null).Count();
            var contacts = _contactService.GetContacts(contactViewModel.ContactFilter, contactViewModel.ContactInput, null, pageIndex);
            var viewModel = new ContactViewModel(_customerService)
            {
                Contacts = new PaginatedList<CustomerContact>(contacts, totalContacts, pageIndex, PageSize)
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult SaveContact(CustomerViewModel viewModel)
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

        [HttpGet]
		public IActionResult PagedData( int customerId ,int pageIndex = 1)
		{
			if (pageIndex < 1)
			{
				pageIndex = 1;
			}

            var data = _contactService.GetContacts(customerId,pageIndex);
			return PartialView("_ContactList",data);
		}

	}
}
