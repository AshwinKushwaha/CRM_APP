using CRMApp.Areas.Identity.Data;
using CRMApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRMApp.Services
{
	public interface IActivityLogger
	{
		void LogAsync(Module moduleId, string userId, string action, bool? isDeleted);
		List<ActivityLog> GetAllActivityLogs();
		string GetUserName(string userId);
		List<ActivityLog> GetActivityLogsByCurrentUser();
		Task<ApplicationUser> GetCurrentUserAsync();

	}
	public class ActivityLogService : IActivityLogger
	{
		private readonly ApplicationUserIdentityContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ActivityLogService(ApplicationUserIdentityContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public List<ActivityLog> GetAllActivityLogs()
		{
			
			return _context.ActivityLogs.Include(log => log.User).OrderByDescending(log => log.TimeStamp).ToList();
		}

		public  void LogAsync(Module moduleId, string userId, string action, bool? isDeleted)
		{
			var log = new ActivityLog
			{
				ModuleId = moduleId,
				UserId = userId,
				Action = action,
				TimeStamp = DateTime.Now,
				IsDeleted = isDeleted,
				Username = _userManager.FindByIdAsync(userId).Result.UserName
			};

			_context.ActivityLogs.Add(log);
			 
		}
		public string GetUserName(string userId)
		{
			return _userManager.FindByIdAsync(userId).Result.UserName;
		}

		public List<ActivityLog> GetActivityLogsByCurrentUser()
		{
			return _context.ActivityLogs.Where(c => c.UserId == GetCurrentUserAsync().Result.Id).OrderByDescending(log => log.TimeStamp).ToList();
		}

		public async Task<ApplicationUser> GetCurrentUserAsync()
		{
			var userPrincipal = _httpContextAccessor.HttpContext?.User;
			return await _userManager.GetUserAsync(userPrincipal);
		}
	}
}
