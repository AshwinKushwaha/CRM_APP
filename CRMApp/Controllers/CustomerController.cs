using CRMApp.Models;
using CRMApp.Services;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IContactService _contactService;
        private readonly INoteService _noteService;
        private readonly int CustomerPageSize = 8;
        private readonly int ContactPageSize = 5;
        private readonly int NotePageSize = 5;



        public CustomerController(ICustomerService customerService, IContactService contactService, INoteService noteService)
        {
            _customerService = customerService;
            _contactService = contactService;
            _noteService = noteService;
        }

        [Authorize]
        public IActionResult Index(int pageIndex = 1)
        {
            var customers = _customerService.GetCustomers(pageIndex);
            var totalCustomer = _customerService.GetCustomerCount();

            var viewModel = new SearchViewModel
            {
                Customers = new PaginatedList<Customer>(customers, totalCustomer, pageIndex, CustomerPageSize)
            };
            ViewBag.Count = viewModel.Customers.Count;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(SearchViewModel viewModel, int pageIndex = 1)
        {
            var result = _customerService.GetCustomers(viewModel.CustomerFilter, viewModel.Input, pageIndex);
            var totalCustomer = _customerService.GetCustomers(viewModel.CustomerFilter, viewModel.Input).Count();
            ViewBag.Count = result.Count;
            var searchViewModel = new SearchViewModel
            {
                Customers = new PaginatedList<Customer>(result, totalCustomer, pageIndex, CustomerPageSize)
            };

            return View(searchViewModel);
        }


        [Authorize(Roles = "admin, salesrep")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name, Email, Phone")] Customer cust)
        {
            if (ModelState.IsValid)
            {
                _customerService.UpsertCustomer(cust);
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, salesrep")]
        public IActionResult Edit(int id)
        {
            var cust = _customerService.GetCustomer(id);
            if (cust == null)
            {
                return NotFound();
            }
            return View(cust);
        }

        [HttpPost]
        public IActionResult Edit(int? id, Customer cust)
        {
            if (id != cust.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _customerService.UpsertCustomer(id, cust);
                return RedirectToAction("Index", "Customer");
            }

            return View(cust);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!_customerService.DeleteCustomer(id))
            {
                return NotFound();
            }


            return RedirectToAction("Index", "Customer");
        }


        [Authorize]
        public IActionResult Details(int id, int ContactPageIndex = 1, int NotePageIndex = 1)
        {


            var cust = _customerService.GetCustomer(id);
            var totalContacts = _contactService.GetContacts(id).Count();
            var contacts = _contactService.GetContacts(id, ContactPageIndex);
            var toatlNotes = _noteService.GetNoteByCustomerId(cust.Id).Count();
            var notes = _noteService.GetNoteByCustomerId(cust.Id, NotePageIndex);
            var cnt = new CustomerContact();
            if (cust == null)
            {
                return NotFound();
            }
            ViewBag.CurrentUser = _contactService.GetCurrentUserAsync().Result.NormalizedUserName;

            var viewModel = new CustomerViewModel()
            {
                Customer = cust,
                CustomerContact = cnt,
                Contacts = new PaginatedList<CustomerContact>(contacts, totalContacts, ContactPageIndex, ContactPageSize),
                Notes = new PaginatedList<Note>(notes, toatlNotes, NotePageIndex, NotePageSize)
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Details(CustomerViewModel viewModel, int id, int ContactPageIndex = 1, int NotePageIndex = 1)
        {
            var cust = _customerService.GetCustomer(id);
            var contacts = new List<CustomerContact>();
            int totalContacts;
            int totalNotes;
            var notes = new List<Note>();

            if (viewModel.ContactFilter != null)
            {
                totalContacts = _contactService.GetContacts((ContactFilter)viewModel.ContactFilter, viewModel.ContactInput, id).Count();
                contacts = _contactService.GetContacts((ContactFilter)viewModel.ContactFilter, viewModel.ContactInput, id, ContactPageIndex);
            }
            else
            {
                totalContacts = _contactService.GetContacts(id).Count();
                contacts = _contactService.GetContacts(id, ContactPageIndex);
            }

            if (viewModel.NoteFilter != null)
            {
                totalNotes = _noteService.GetNotes((NoteFilter)viewModel.NoteFilter, viewModel.NoteInput, id).Count();
                notes = _noteService.GetNotes((NoteFilter)viewModel.NoteFilter, viewModel.NoteInput, id);
            }
            else
            {
                totalNotes = _noteService.GetNoteByCustomerId(id, NotePageIndex).Count();
                notes = _noteService.GetNoteByCustomerId(id, NotePageIndex);
            }



            var cnt = new CustomerContact();
            ViewBag.CurrentUser = _contactService.GetCurrentUserAsync().Result.NormalizedUserName;

            var customerViewModel = new CustomerViewModel(_customerService)
            {
                Customer = cust,
                CustomerContact = cnt,
                Contacts = new PaginatedList<CustomerContact>(contacts, totalContacts, ContactPageIndex, ContactPageSize),
                Notes = new PaginatedList<Note>(notes, totalNotes, NotePageIndex, NotePageSize)
            };
            return View(customerViewModel);
        }


    }


}
