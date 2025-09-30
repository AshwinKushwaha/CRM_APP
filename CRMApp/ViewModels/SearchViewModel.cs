using CRMApp.Models;

namespace CRMApp.ViewModels
{
	public class SearchViewModel
	{
		public List<Customer> Customers {  get; set; }

		public Filter filter { get; set; }

		public Customer Customer { get; set; }

		public string Input { get; set; }
	}
	public enum Filter
	{
		All,
		Name,
		Email,
		Phone
	}

}
