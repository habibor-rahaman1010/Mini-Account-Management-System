using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class ChartOfAccountModel
    {
        [Required, Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public Guid? ParentId { get; set; }
    }
}
