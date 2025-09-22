using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationUserIdentityContext context;


        public CustomerController(ApplicationUserIdentityContext context)
        {
            this.context = context;
        }

        [Authorize(Roles = "admin , salesrep, support")]
        public IActionResult Index()
        {
            var cust = context.Customers.ToList();
            ViewBag.Count = cust.Count;
            return View(cust);
        }


        [Authorize(Roles = "admin, salesrep")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer cust)
        {
            if (ModelState.IsValid)
            {
                context.Add(cust);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, salesrep")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
