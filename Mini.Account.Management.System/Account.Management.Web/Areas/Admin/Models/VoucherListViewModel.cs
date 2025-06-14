﻿using Account.Management.Domain.Dtos;
using Account.Management.Web.Utitlity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherListViewModel
    {
        public IEnumerable<VoucherDto>? Vouchers { get; set; }
        public Pager? Pager { get; set; }
    }
}
