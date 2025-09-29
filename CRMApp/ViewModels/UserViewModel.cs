using CRMApp.Areas.Identity.Data;

namespace CRMApp.ViewModels
{
    public class UserViewModel
    {
       public List<ApplicationUser> Users {  get; set; }

        public ApplicationUser User { get; set; }
    }
}
