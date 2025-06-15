using Account.Management.Infrastructure.Account.Management.Identity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }

        public UserProfileViewModel()
        {
            User = new ApplicationUser();
            Roles = new List<string>();
        }
    }
}
