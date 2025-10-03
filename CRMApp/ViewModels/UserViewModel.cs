using CRMApp.Areas.Identity.Data;

namespace CRMApp.ViewModels
{
    public class UserViewModel
    {
       public List<ApplicationUser> Users {  get; set; }

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
