using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Web.Utitlity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class RoleListViewModel
    {
        public IEnumerable<ApplicationRole> Roles { get; set; }
        public Pager Pager { get; set; }
    }
}
