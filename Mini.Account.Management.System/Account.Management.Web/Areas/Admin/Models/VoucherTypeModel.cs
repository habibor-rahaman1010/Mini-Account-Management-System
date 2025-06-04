using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherTypeModel
    {
        [Required, Display(Name = "Type Name")]
        public string TypeName { get; set; }
    }
}
