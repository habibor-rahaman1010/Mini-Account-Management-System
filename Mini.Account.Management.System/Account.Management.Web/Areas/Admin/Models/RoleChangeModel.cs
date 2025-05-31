using Microsoft.AspNetCore.Mvc.Rendering;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class RoleChangeModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public IList<SelectListItem> Users { get; set; }
        public IList<SelectListItem> Roles { get; set; }

        public RoleChangeModel()
        {
            Users = new List<SelectListItem>();
            Roles = new List<SelectListItem>();
        }
    }
}
