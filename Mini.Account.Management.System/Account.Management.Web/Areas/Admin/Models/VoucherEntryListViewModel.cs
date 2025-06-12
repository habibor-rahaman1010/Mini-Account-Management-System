using Account.Management.Domain.Dtos;
using Account.Management.Web.Utitlity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class VoucherEntryListViewModel
    {
        public IEnumerable<VoucherEntryDto>? VoucherEntries { get; set; }
        public Pager? Pager { get; set; }
    }
}
