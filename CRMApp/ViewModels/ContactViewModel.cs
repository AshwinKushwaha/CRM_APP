using CRMApp.Models;
using CRMApp.Services;

namespace CRMApp.ViewModels
{
	public class ContactViewModel
	{
		private readonly ICustomerService _customerService;

		public ContactViewModel(ICustomerService customerService)
        {
			_customerService = customerService;
		}
        public List<CustomerContact> Contacts { get; set; }
		public CustomerContact customerContact { get; set; }

		public string CustomerName(int customerId)
		{
			return _customerService.GetCustomer(customerId).Name;
		}
	}
}
