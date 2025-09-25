using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Services
{
	public interface IContactService
	{
		List<CustomerContact> GetContacts(int id);
		CustomerContact GetContact(int id);
		Task CreateContact(CustomerContact contact);
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

		public async Task CreateContact(CustomerContact contact)
		{

			var userId = GetCurrentUserAsync();
            if (contact.Id == 0)
			{
				context.CustomerContacts.Add(contact);
				await _activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Added contact: {contact.CustName}",false);
				
			}
			else
			{
				context.CustomerContacts.Update(contact);
				await _activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Updated contact: {contact.CustName}", false);
				
			}
			await context.SaveChangesAsync();
			

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
				context.SaveChanges();
				_activityLogger.LogAsync(Module.Contact, userId.Result.Id, $"Deleted contact: {deletedContactName}", true);
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
	}
}
