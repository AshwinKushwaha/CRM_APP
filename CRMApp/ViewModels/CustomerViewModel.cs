using CRMApp.Models;
using CRMApp.Services;

namespace CRMApp.ViewModels
{
    public class CustomerViewModel
    {
		private readonly ICustomerService customerService;

		public CustomerViewModel(){}

        public CustomerViewModel(ICustomerService customerService)
        {
			this.customerService = customerService;
		}
        public Customer Customer { get; set; }
        public Note Note { get; set; }
        public CustomerContact CustomerContact { get; set; }

        public List<CustomerContact> Contacts { get; set; }

        public List<Note> Notes { get; set; }

		public string FormatDate(DateTime date)
		{
			string suffix = (date.Day >= 11 && date.Day <= 13) ? "th" :
				date.Day % 10 == 1 ? "st" :
				date.Day % 10 == 2 ? "nd" :
				date.Day % 10 == 3 ? "rd" : "th";

			return $"{date.Day}{suffix} {date:MMMM}, {date:yyyy}";
		}
		
		public string CurrentUsername { get; set; }
		
		public ContactFilter? ContactFilter { get; set; }
		
		public NoteFilter? NoteFilter { get; set; }

		public string? ContactInput { get; set; }
		public string? NoteInput {  get; set; }
	}

	public enum ContactFilter
	{
		All,
		ContactName,
		Contact,
		Relation,
		ContactType
	}

	public enum NoteFilter
	{
		All,
		Description
	}
}
