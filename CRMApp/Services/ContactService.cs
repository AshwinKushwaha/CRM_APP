using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Services
{
	public interface IContactService
	{
		List<CustomerContact> GetContacts(int id);
		CustomerContact GetContact(int id);
		bool CreateContact(CustomerContact contact);
		bool DeleteContact(int id);
	}
	public class ContactService : IContactService
	{
		private readonly ApplicationUserIdentityContext context;

		public ContactService(ApplicationUserIdentityContext context)
        {
			this.context = context;
		}

		public List<CustomerContact> GetContacts(int id)
		{
			return context.CustomerContacts.Where(c => c.CustomerId == id).ToList();
		}

		public bool CreateContact(CustomerContact contact)
		{
            if (contact.Id == 0)
                context.CustomerContacts.Add(contact);
            else
                context.CustomerContacts.Update(contact);

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
			if (contact == null)
			{
				return false;
			}
			context.CustomerContacts.Remove(contact);
			context.SaveChanges();
			return true;
		}
	}
}
