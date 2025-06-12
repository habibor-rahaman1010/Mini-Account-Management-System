using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.Dtos
{
    public class VoucherEntryDto
    {
        public Guid Id { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string ReferenceNo { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string AccountName { get; set; } = string.Empty;
        public string VoucherTypeName { get; set; } = string.Empty;
    }
}
