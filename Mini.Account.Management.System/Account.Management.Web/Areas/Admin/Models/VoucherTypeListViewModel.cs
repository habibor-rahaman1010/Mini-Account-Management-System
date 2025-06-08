using Account.Management.Domain.Dtos;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Web.Utitlity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherTypeListViewModel
    {
        public IEnumerable<VoucherTypeDto>? VoucherTypes { get; set; }
        public Pager? Pager { get; set; }
    }
}
