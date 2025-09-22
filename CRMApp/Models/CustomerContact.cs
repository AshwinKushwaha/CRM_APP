using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMApp.Models
{
	public class CustomerContact
	{
		[Key]
		public int Id { get; set; }
		//[ForeignKey()]
		public int CustomerId { get; set; }
		public string Name { get; set; }
		public Relation Relation { get; set; } 
		public Type ContactType { get; set; }
		public string Contact { get; set; }

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
