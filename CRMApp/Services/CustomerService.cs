using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRMApp.Services
{
	public interface ICustomerService
	{
		List<Customer> GetCustomers(string? input);
		List<Customer> GetCustomers();
		Customer GetCustomer(int id);
		bool UpsertCustomer(int? id, Customer customer);
		bool UpsertCustomer(Customer customer);

		bool DeleteCustomer(int id);

		


	}
	public class CustomerService : ICustomerService
	{
		private readonly ApplicationUserIdentityContext context;

		public CustomerService(ApplicationUserIdentityContext context)
        {
			this.context = context;
		}

		

		public bool DeleteCustomer(int id)
		{
			var cust = GetCustomer(id);
			if (cust == null)
			{
				return false;
			}
			context.Customers.Remove(cust);
			context.SaveChanges();

			return true;
		}

		public Customer GetCustomer(int id)
		{

			return context.Customers.Find(id);
		}

		public List<Customer> GetCustomers(string? input)
		{
			
				return context.Customers.Where(c => (!string.IsNullOrEmpty(input))
				&& (c.Name.Contains(input)) ||
				(c.Email.Contains(input)) ||
				(c.Phone.Contains(input))
				).ToList();
			
			
		}

		public List<Customer> GetCustomers()
		{
			return context.Customers.ToList();
		}

		public bool UpsertCustomer(int? id, Customer customer) // for updation
		{
			
			 if (customer.Id > 0)
			{
				customer.UpdatedAt = DateTime.Now;
				context.Customers.Update(customer);
				context.SaveChanges();
			}
			
			return true;
			
		}

		public bool UpsertCustomer(Customer customer) // for creation
		{
			if (customer.Id == 0)
			{
				customer.CreatedAt = DateTime.Now;
				context.Customers.Add(customer);
				context.SaveChanges();
			}
			return true;
		}
	}
}
