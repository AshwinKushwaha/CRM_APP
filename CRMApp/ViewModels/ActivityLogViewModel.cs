using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using CRMApp.Services;
using Microsoft.AspNetCore.Identity;

namespace CRMApp.ViewModels
{
	public class ActivityLogViewModel
	{
		
		private readonly IContactService _contactService;
		private string _userName;

		public ActivityLogViewModel(IContactService contactService)
        {
			
			_contactService = contactService;
		}
        public List<ActivityLog> activityLogs {  get; set; }

		//public string CurrentUsername
		//{
		//	get
		//	{
		//		return _userName;
		//	}

		//	set
		//	{
		//		_userName = _contactService.GetCurrentUserAsync().Result.UserName;
		//	}
		//}

		public string UserName { get; set; }



		public string FormatDate(DateTime date)
		{
			string suffix = (date.Day >= 11 && date.Day <= 13) ? "th" :
				date.Day % 10 == 1 ? "st" :
				date.Day % 10 == 2 ? "nd" :
				date.Day % 10 == 3 ? "rd" : "th";

			return $"{date.Day}{suffix} {date:MMMM}, {date:yyyy}";
		}
	}
}
