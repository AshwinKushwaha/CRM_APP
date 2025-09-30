using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Services
{
	public interface IContactService
	{
		List<CustomerContact> GetContacts(int id);
		List<CustomerContact> GetContacts(ContactFilter contactFilter,string input,int id);
		List<CustomerContact> GetAllContacts();
		CustomerContact GetContact(int id);
		bool CreateContact(CustomerContact contact);
		bool DeleteContact(int id);
		int GetCount();
		Task<ApplicationUser> GetCurrentUserAsync();
		//string GetUserName();
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
            if (contact.Id == 0)
			{
				context.CustomerContacts.Add(contact);
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Added by {userId.Result.NormalizedUserName} ({contact.CustName})",false);
				
			}
			else
			{
				context.CustomerContacts.Update(contact);
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Updated by {userId.Result.NormalizedUserName} ({contact.CustName})", false);
				
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
			var deletedContactName = contact.CustName;
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

		public List<CustomerContact> GetContacts(ContactFilter contactFilter, string input, int customerId)
		{
			switch (contactFilter)
			{
				case ContactFilter.Contact:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.Contact.Contains(input))).ToList();
				case ContactFilter.CustName:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.CustName.Contains(input))).ToList();
				//case ContactFilter.Relation:
				//	return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.Relation.ToString().Contains(input))).ToList();
				//case ContactFilter.ContactType:
				//	return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && (c.CustomerId == customerId) && (c.ContactType.ToString().Contains(input))).ToList();
				case ContactFilter.All:
				default:
					return context.CustomerContacts.Where(c => (!string.IsNullOrEmpty(input)) && 
					(c.CustomerId == customerId) && 
					
					(c.CustName.Contains(input)) ||
					(c.Contact.Contains(input)) 
					
					).ToList();

			}
		}
	}
}
