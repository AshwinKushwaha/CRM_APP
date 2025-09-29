using CRMApp.Models;

namespace CRMApp.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public Note Note { get; set; }
        public CustomerContact CustomerContact { get; set; }

        public List<CustomerContact> Contacts { get; set; }

        public List<Note> Notes { get; set; }
    }
}
