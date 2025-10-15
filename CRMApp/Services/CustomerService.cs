using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.ViewModels;

namespace CRMApp.Services
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers(CustomerFilter filter, string? input, int pageIndex);
        List<Customer> GetCustomers(CustomerFilter filter, string? input);
        List<Customer> GetCustomers();
        List<Customer> GetCustomers(int pageIndex);
        Customer GetCustomer(int id);
        bool UpsertCustomer(int? id, Customer customer);
        bool UpsertCustomer(Customer customer);

        bool DeleteCustomer(int id);

        int GetCustomerCount();
        int GetUserCount();



    }
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationUserIdentityContext context;
        private readonly IActivityLogger _activityLogger;
        private readonly IContactService _contactService;
        private readonly int PageSize = 8;

        public CustomerService(ApplicationUserIdentityContext context, IActivityLogger activityLogger, IContactService contactService)
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
        public int GetCustomerCount()
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




        public List<Customer> GetCustomers(CustomerFilter filter, string? input, int pageIndex)
        {
            switch (filter)
            {
                case CustomerFilter.Name:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Name.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

                case CustomerFilter.Email:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Email.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

                case CustomerFilter.Phone:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Phone.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

                case CustomerFilter.All:

                default:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input))
                && (c.Name.Contains(input)) ||
                (c.Email.Contains(input)) ||
                (c.Phone.Contains(input))
                )
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
            }


        }


        public bool UpsertCustomer(int? id, Customer customer) // for updation
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

        public bool UpsertCustomer(Customer customer) // for creation
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

        public List<Customer> GetCustomers(int pageIndex)
        {
            return context.Customers.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        public List<Customer> GetCustomers(CustomerFilter filter, string? input)
        {
            switch (filter)
            {
                case CustomerFilter.Name:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Name.Contains(input)))
                        .ToList();

                case CustomerFilter.Email:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Email.Contains(input)))
                        .ToList();

                case CustomerFilter.Phone:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input)) && (c.Phone.Contains(input)))
                        .ToList();

                case CustomerFilter.All:

                default:
                    return context.Customers.Where(c => (!string.IsNullOrEmpty(input))
                && (c.Name.Contains(input)) ||
                (c.Email.Contains(input)) ||
                (c.Phone.Contains(input))
                )
                        .ToList();
            }
        }
    }
}
