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

		public ContactController(IContactService contactService,ICustomerService customerService)
        {
            _contactService = contactService;
			_customerService = customerService;
		}
        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
				List<CustomerContact> contacts = _contactService.GetAllContacts();
                var viewModel = new ContactViewModel(_customerService)
                {
                    Contacts = contacts
                };
                return View(viewModel);
			}
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel contactViewModel)
        {
            var contacts = _contactService.GetContacts(contactViewModel.ContactFilter, contactViewModel.ContactInput,null);
            var viewModel = new ContactViewModel(_customerService)
            {
                Contacts = contacts
            };
            return View(viewModel);
        }


        [HttpPost]
        public  IActionResult SaveContact(CustomerViewModel viewModel)
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
