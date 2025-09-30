using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using CRMApp.ViewModels;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
		private readonly ICustomerService _customerService;
		private readonly IContactService _contactService;
		private readonly INoteService _noteService;

		//private readonly ApplicationUserIdentityContext context;


		public CustomerController(ICustomerService customerService,IContactService contactService, INoteService noteService)
        {
			_customerService = customerService;
			_contactService = contactService;
			_noteService = noteService;
			//this.context = context;
		}

        [Authorize]
        public IActionResult Index()
        {
            var cust = _customerService.GetCustomers();
            ViewBag.Count = cust.Count;
            return View(cust);
        }

        [HttpPost]
		public IActionResult Index(string input)
        {
            var result = _customerService.GetCustomers(input);
			ViewBag.Count = result.Count;

			return View(result);
        }

		//[HttpGet("{id?}")]
		[Authorize(Roles = "admin, salesrep")]
        public IActionResult Create()
        {
            //Customer cust = new Customer();
            //if (id != null)
            //{
            //    cust = context.Customers.First(p => p.Id == id);
            //}

            return View();
        }

        [HttpPost]
        public   IActionResult Create([Bind("Name, Email, Phone")]Customer cust)
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
        public  IActionResult Edit(int? id, Customer cust)
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
        public IActionResult Details(int id)
        {
           
            var cust = _customerService.GetCustomer(id);
            var contacts = _contactService.GetContacts(id);
            var notes = _noteService.GetNoteByCustomerId(cust.Id);
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
                Contacts = contacts,
                Notes = notes
            };
            return View(viewModel);
        }


		

	}
}
