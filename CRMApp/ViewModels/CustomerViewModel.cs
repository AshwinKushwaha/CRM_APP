using CRMApp.Models;

namespace CRMApp.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public CustomerContact CustomerContact { get; set; }

        public List<CustomerContact> Contacts { get; set; }
    }
}
