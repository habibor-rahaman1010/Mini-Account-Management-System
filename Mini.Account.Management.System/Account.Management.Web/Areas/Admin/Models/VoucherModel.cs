using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Voucher date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Required(ErrorMessage = "Reference number is required.")]
        [StringLength(100)]
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }

        [Required(ErrorMessage = "Voucher type is required.")]
        [Display(Name = "Voucher Type")]
        public Guid VoucherTypeId { get; set; }
    }
}
