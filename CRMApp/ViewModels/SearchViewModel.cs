using CRMApp.Models;

namespace CRMApp.ViewModels
{
	public class SearchViewModel
	{
		public PaginatedList<Customer> Customers {  get; set; }

		public CustomerFilter CustomerFilter { get; set; }

		public Customer Customer { get; set; }

		public string Input { get; set; }
	}
	public enum CustomerFilter
	{
		All,
		Name,
		Email,
		Phone
	}

}
