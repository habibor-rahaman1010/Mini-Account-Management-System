using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherUpdateModel
    {
        [Required, Display(Name = "Type Name")]
        public string TypeName { get; set; }
    }
}
