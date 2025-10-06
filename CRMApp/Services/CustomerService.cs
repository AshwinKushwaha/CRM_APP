using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.ViewModels;

namespace CRMApp.Services
{
	public interface ICustomerService
	{
		List<Customer> GetCustomers(Filter filter,string? input);
		List<Customer> GetCustomers();
		Customer GetCustomer(int id);
		bool UpsertCustomer(int? id, Customer customer);
		bool UpsertCustomer(Customer customer);

		bool DeleteCustomer(int id);

		int GetCount();
		int GetUserCount();
		


	}
	public class CustomerService : ICustomerService
	{
		private readonly ApplicationUserIdentityContext context;
		private readonly IActivityLogger _activityLogger;
		private readonly IContactService _contactService;

		public CustomerService(ApplicationUserIdentityContext context,IActivityLogger activityLogger,IContactService contactService)
        {
			this.context = context;
			_activityLogger = activityLogger;
			_contactService = contactService;
		}



		public Customer GetCustomer(int id)
		{
			return context.Customers.Find(id);
		}

		public List<Customer> GetCustomers()
		{
			return context.Customers.ToList();
		}

		public int GetUserCount()
		{
			return context.Users.Count();
		}
		public int GetCount()
		{
			return context.Customers.Count();
		}

		public bool DeleteCustomer(int id)
		{
			var cust = GetCustomer(id);
			var deletedCustomerName = cust.Name;
			var userId = _contactService.GetCurrentUserAsync();
			
			if (cust == null)
			{
				return false;
			}
			else
			{
				context.Customers.Remove(cust);
				_activityLogger.LogAsync(Module.Customer, userId.Result.Id, $"Deleted by {userId.Result.NormalizedUserName} ({deletedCustomerName})", true);
				context.SaveChanges();

				return true;
			}
			
		}

		

		
		public List<Customer> GetCustomers(Filter filter,string? input)
		{
			switch (filter)
			{
				case Filter.Name:
					return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Name.Contains(input))).ToList();

				case Filter.Email:
					return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Email.Contains(input))).ToList();

				case Filter.Phone:
					return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Phone.Contains(input))).ToList();

				case Filter.All:

				default:
					return context.Customers.Where(c => (!string.IsNullOrEmpty(input))
				&& (c.Name.Contains(input)) ||
				(c.Email.Contains(input)) ||
				(c.Phone.Contains(input))
				).ToList();
			}
			
			
		}


		public  bool UpsertCustomer(int? id, Customer customer) // for updation
		{
			var userId = _contactService.GetCurrentUserAsync();
			if (customer.Id > 0)
			{
				customer.UpdatedAt = DateTime.Now;
				context.Customers.Update(customer);
				 _activityLogger.LogAsync(Module.Customer, userId.Result.Id, $"Updated by {userId.Result.NormalizedUserName} ({customer.Name})", false);
				 context.SaveChanges();
			}

			return true;
			
		}

		public  bool UpsertCustomer(Customer customer) // for creation
		{
			var userId = _contactService.GetCurrentUserAsync();
			if (customer.Id == 0)
			{
				customer.CreatedAt = DateTime.Now;
				context.Customers.Add(customer);
				 _activityLogger.LogAsync(Module.Customer, userId.Result.Id, $"Added by {userId.Result.NormalizedUserName} ({customer.Name})", false);
				 context.SaveChanges();
			}
			return true;
		}
	}
}
