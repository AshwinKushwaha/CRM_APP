using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using System.Linq;

namespace CRMApp.Services
{
    public interface IContactInquiryService
    {
        List<ContactInquiry> GetInquiries();
        bool SaveInquiry(ContactInquiry contactInquiry);
        List<ContactInquiry> GetAllInquiries();
        ContactInquiry GetInquiry(int id);
    }
    public class ContactInquiryService : IContactInquiryService
    {
        private readonly ApplicationUserIdentityContext _context;

        public ContactInquiryService(ApplicationUserIdentityContext context)
        {
            _context = context;
        }

		public List<ContactInquiry> GetAllInquiries()
		{
			return _context.ContactInquiries.OrderByDescending(c => c.CreatedAt).ToList();
		}

		public List<ContactInquiry> GetInquiries()
        {
           return _context.ContactInquiries.Where(c => c.isArchived == false).OrderByDescending(c => c.CreatedAt).Take(3).ToList();
        }

		public ContactInquiry GetInquiry(int id)
		{
            return _context.ContactInquiries.FirstOrDefault(c => c.Id == id);
		}

		public bool SaveInquiry(ContactInquiry contactInquiry)
		{
            if (contactInquiry == null) {
                return false;
                    }
            if(contactInquiry.Id == 0)
            {
				contactInquiry.CreatedAt = DateTime.Now;
				_context.ContactInquiries.Add(contactInquiry);
            }
            else
            {
                _context.Update(contactInquiry);
            }
            _context.SaveChanges();
            return true;
		}
	}
}
