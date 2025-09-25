using CRMApp.Areas.Identity.Data;
using CRMApp.Models;

namespace CRMApp.Services
{
	public interface IActivityLogger
	{
		Task LogAsync(Module moduleId, string userId, string action, bool? isDeleted);
	}
	public class ActivityLogService : IActivityLogger
	{
		private readonly ApplicationUserIdentityContext _context;

		public ActivityLogService(ApplicationUserIdentityContext context)
		{
			_context = context;
		}

		public async Task LogAsync(Module moduleId, string userId, string action, bool? isDeleted)
		{
			var log = new ActivityLog
			{
				ModuleId = moduleId,
				UserId = userId,
				Action = action,
				TimeStamp = DateTime.UtcNow,
				IsDeleted = isDeleted
			};

			_context.ActivityLogs.Add(log);
			await _context.SaveChangesAsync();

			//}
		}
	}
}
