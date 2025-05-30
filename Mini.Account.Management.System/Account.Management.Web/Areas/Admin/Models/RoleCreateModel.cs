using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class RoleCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
