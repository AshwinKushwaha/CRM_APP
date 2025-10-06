using CRMApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMApp.Models
{
	public class ActivityLog
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }

		public Module ModuleId { get; set; }

		public string Action { get; set; }

		public DateTime TimeStamp { get; set; }

		public bool? IsDeleted { get; set; }
		
		public string Username { get; set; }


		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }

	}

	public enum Module
	{
		User,
		Customer,
		Contact,
		Note
	}
}
