using CRMApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CRMApp.Services
{
    public interface IUserService
    {
        List<ApplicationUser> GetAllUsers();
    }
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }
    }
}
