using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CRMApp.Models
{
	public class CustomerContact
	{
		[Key]
		public int Id { get; set; }

		
		public int CustomerId { get; set; }
		
		[Required(ErrorMessage = "Customer Name is required")]
		
		public string CustName { get; set; }

		[Required]
		public Relation Relation { get; set; } 

		[Required]
		public Type ContactType { get; set; }

		[Required(ErrorMessage = "Contact is Required")]
		public string Contact { get; set; }

		[ForeignKey("CustomerId")]
		[JsonIgnore]
		public Customer Customer { get; set; }

	}

	public enum Relation
	{
		Self = 0,
		Father,
		Mother,
		Friend,
		Others
	}

	public enum Type
	{
		Mobile,
		Telephone,
		Email,
		Address
	}
}
