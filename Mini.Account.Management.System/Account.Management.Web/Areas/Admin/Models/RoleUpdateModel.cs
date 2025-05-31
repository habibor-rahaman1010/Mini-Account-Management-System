using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class RoleUpdateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
