using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CRMApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationUserIdentityContext context;


        public CustomerController(ApplicationUserIdentityContext context)
        {
            this.context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var cust = context.Customers.ToList();
            ViewBag.Count = cust.Count;
            return View(cust);
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
        public IActionResult Create([Bind("Name, Email, Phone")]Customer cust)
        {
            if (ModelState.IsValid)
            {
                cust.CreatedAt = DateTime.Now;
                context.Add(cust);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "admin, salesrep")]
        public IActionResult Edit(int? id)
        { 
            if(id == null)
            {

                return NotFound();
            }

            var cust = context.Customers.Find(id);
            if(cust == null)
            {
                return NotFound();
            }
            return View(cust);
        }

        [HttpPost]
        public IActionResult Edit(int? id, Customer cust)
        {
            if(id != cust.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                cust.UpdatedAt = DateTime.Now;
                context.Customers.Update(cust);
                context.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }

            return View(cust);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var cust = context.Customers.Find(id);
            if(cust == null)
            {
                return NotFound();
            }

            context.Customers.Remove(cust);
            context.SaveChanges();

            return RedirectToAction("Index","Customer");
        }



        public IActionResult Details(int id)
        {
            return View();
        }
        
    }
}
