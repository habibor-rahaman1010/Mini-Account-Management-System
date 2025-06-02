using Account.Management.Domain.Dtos;
using Account.Management.Web.Utitlity;

namespace Account.Management.Web.Areas.Admin.Models
{
    public class ChartOfAccountListViewModel
    {
        public IEnumerable<ChartOfAccountDto> chartOfAccountDtos { get; set; }
        public Pager Pager { get; set; }
    }
}
