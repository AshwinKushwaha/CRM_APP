using CRMApp.Areas.Identity.Data;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IContactInquiryService
    {
        List<ContactInquiry> GetInquiries();
        bool SaveInquiry(ContactInquiry contactInquiry);
        List<ContactInquiry> GetAllInquiries();
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
           return _context.ContactInquiries.OrderByDescending(c => c.CreatedAt).Take(3).ToList();
        }

		public bool SaveInquiry(ContactInquiry contactInquiry)
		{
            if (contactInquiry == null) {
                return false;
                    }
            contactInquiry.CreatedAt = DateTime.Now;
            _context.ContactInquiries.Add(contactInquiry);
            _context.SaveChanges();
            return true;
		}
	}
}
