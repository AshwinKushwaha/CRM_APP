using CRMApp.Models;
using CRMApp.Services;
using System.ComponentModel.DataAnnotations;

namespace CRMApp.ViewModels
{
    public class ContactViewModel
    {
        private readonly ICustomerService _customerService;

        public ContactViewModel() { }

        public ContactViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public PaginatedList<CustomerContact>? Contacts { get; set; }
        public CustomerContact? customerContact { get; set; }

        public ContactFilter ContactFilter { get; set; }

        [Required(ErrorMessage = "Search Input should not be Empty...")]
        public string? ContactInput { get; set; }


        public string CustomerName(int customerId)
        {
            return _customerService.GetCustomer(customerId).Name;
        }
    }
}
