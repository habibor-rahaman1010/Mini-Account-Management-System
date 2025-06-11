using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.Dtos
{
    public class VoucherDto
    {
        public Guid Id { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime VoucherUpdateAt { get; set; }
        public string? ReferenceNo { get; set; }
        public string? VoucherTypeName { get; set; }
    }
}
