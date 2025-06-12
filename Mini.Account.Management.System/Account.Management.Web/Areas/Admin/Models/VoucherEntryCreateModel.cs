using System.ComponentModel.DataAnnotations;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherEntryCreateModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debit Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Debit Amount must be greater than 0.")]
        [Display(Name = "Debit Amount")]
        public decimal? DebitAmount { get; set; }

        [Required(ErrorMessage = "Credit Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Credit Amount must be greater than 0.")]
        [Display(Name = "Credit Amount")]
        public decimal? CreditAmount { get; set; }

        [Required(ErrorMessage = "Reference no is required.")]
        public string ReferenceNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Narration is required.")]
        public string Narration { get; set; } = string.Empty;

        [Required(ErrorMessage = "Created Date is required.")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Chart of account is required.")]
        [Display(Name = "Chart Of Account")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Voucher is required.")]
        [Display(Name = "Voucher Type")]
        public Guid VoucherTypeId { get; set; }
    }
}
