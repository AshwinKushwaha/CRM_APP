using CRMApp.Areas.Identity.Data;
using CRMApp.Models;

namespace CRMApp.ViewModels
{
    public class UserViewModel
    {
        public PaginatedList<ApplicationUser> Users { get; set; }

        public UserFilter UserFilter { get; set; }
        public string UserInput { get; set; }

        public ApplicationUser User { get; set; }
    }

    public enum UserFilter
    {
        All,
        Username,
        Email

    }
}
