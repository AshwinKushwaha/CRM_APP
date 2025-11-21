using CRMApp.Areas.Identity.Data;
using CRMApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CRMApp.Services
{
    public interface IUserService
    {
        List<ApplicationUser> GetUsers(int pageIndex);
        int GetUserCount();
        List<ApplicationUser> GetUsers(UserFilter filter, string input);
        bool DeleteUser(string id);
        ApplicationUser GetUser(string id);
        List<ApplicationUser> GetUsers(UserFilter filter, string input, int pageIndex);


    }
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserIdentityContext _context;
        private readonly IActivityLogger _activityLogger;
        private readonly IContactService _contactService;
        private readonly int PageSize = 8;


        public UserService(UserManager<ApplicationUser> userManager, ApplicationUserIdentityContext context, IActivityLogger activityLogger, IContactService contactService)
        {
            _userManager = userManager;
            _context = context;
            _activityLogger = activityLogger;
            _contactService = contactService;
        }

        public bool DeleteUser(string id)
        {
            var user = GetUser(id);
            var deleteUser = user.UserName;
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            _activityLogger.LogAsync(Models.Module.User, _contactService.GetCurrentUserAsync().Result.Id, $"Deleted by {_contactService.GetCurrentUserAsync().Result.NormalizedUserName} ({deleteUser})", true);
            _context.SaveChanges();
            return true;

        }

        public List<ApplicationUser> GetUsers(int pageIndex)
        {
            return _userManager.Users
                .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _userManager.Users.FirstOrDefault(c => c.Id == id);
        }



        public List<ApplicationUser> GetUsers(UserFilter filter, string input)
        {
            switch (filter)
            {
                case UserFilter.Username:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) && (c.UserName.Contains(input))).ToList();

                case UserFilter.Email:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) && (c.Email.Contains(input))).ToList();

                case UserFilter.All:
                default:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) &&
                    (c.Email.Contains(input)) ||
                    (c.UserName.Contains(input))
                    ).ToList();

            }
        }
        public List<ApplicationUser> GetUsers(UserFilter filter, string input, int pageIndex)
        {
            switch (filter)
            {
                case UserFilter.Username:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) && (c.UserName.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

                case UserFilter.Email:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) && (c.Email.Contains(input)))
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

                case UserFilter.All:
                default:
                    return _userManager.Users.Where(c => (!string.IsNullOrEmpty(input)) &&
                    (c.Email.Contains(input)) ||
                    (c.UserName.Contains(input))
                    )
                        .Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();

            }
        }

        public int GetUserCount()
        {
            return _userManager.Users.Count();
        }
    }
}
