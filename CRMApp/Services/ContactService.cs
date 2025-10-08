using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CRMApp.Services
{
	public interface IContactService
	{
		List<CustomerContact> GetContacts(int id);
		List<CustomerContact> GetContacts(ContactFilter contactFilter,string input,int? id);
		List<CustomerContact> GetAllContacts();
		CustomerContact GetContact(int id);
		bool CreateContact(CustomerContact contact);
		bool DeleteContact(int id);
		int GetCount();
		Task<ApplicationUser> GetCurrentUserAsync();
	}
	public class ContactService : IContactService
	{
		private readonly ApplicationUserIdentityContext context;
		private readonly IActivityLogger _activityLogger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ContactService(ApplicationUserIdentityContext context, IActivityLogger activityLogger, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
			this.context = context;
			_activityLogger = activityLogger;
			_userManager = userManager;
			this.httpContextAccessor = httpContextAccessor;
		}

		public List<CustomerContact> GetContacts(int id)
		{
			return context.CustomerContacts.Where(c => c.CustomerId == id).ToList();
		}

		public bool CreateContact(CustomerContact contact)
		{

			var userId = GetCurrentUserAsync();
			if(userId == null)
			{
				Console.WriteLine("User is Null");
				return false;
			}
            if (contact.Id == 0)
			{
				context.CustomerContacts.Add(contact);
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Added by {userId.Result.NormalizedUserName} ({contact.ContactName})",false);
				
			}
			else
			{
				context.CustomerContacts.Update(contact);
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Updated by {userId.Result.NormalizedUserName} ({contact.ContactName})", false);
				
			}
			context.SaveChanges();
			return true;

        }

		public CustomerContact GetContact(int id)
		{
			return context.CustomerContacts.FirstOrDefault(c => c.Id == id);
		}

		public bool DeleteContact(int id)
		{
			var contact = GetContact(id);
			var userId = GetCurrentUserAsync();
			var deletedContactName = contact.ContactName;
			if (contact == null)
			{
				return false;
			}
			else
			{
				context.CustomerContacts.Remove(contact);
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Deleted by {userId.Result.NormalizedUserName} ({deletedContactName})", true);
				context.SaveChanges();
				return true;
			}
			
		}

		public int GetCount()
		{
			return context.CustomerContacts.Count();
		}

		public async Task<ApplicationUser> GetCurrentUserAsync()
		{
			var userPrincipal = httpContextAccessor.HttpContext?.User;
			return await _userManager.GetUserAsync(userPrincipal);
		}

		public List<CustomerContact> GetAllContacts()
		{
			return context.CustomerContacts.ToList();
		}

		public List<CustomerContact> GetContacts(ContactFilter contactFilter, string input, int? customerId)
		{
			if (customerId != null)
			{
				switch (contactFilter)
				{
				case ContactFilter.Contact:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.Contact.Contains(input))).ToList();
				case ContactFilter.CustName:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.ContactName.Contains(input))).ToList();
				case ContactFilter.All:
				default:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && 
					(c.CustomerId == customerId) && 
					
					(c.ContactName.Contains(input)) ||
					(c.Contact.Contains(input)) 
					
					).ToList();
				}
			}
			else
			{
                switch (contactFilter)
                {
                    case ContactFilter.Contact:
                        return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) &&  (c.Contact.Contains(input))).ToList();
                    case ContactFilter.CustName:
                        return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) &&  (c.ContactName.Contains(input))).ToList();
                    case ContactFilter.All:
                    default:
                        return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) &&
                        (c.ContactName.Contains(input)) ||
                        (c.Contact.Contains(input))
                        ).ToList();

                }
            }
		}
	}
}
