using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.Dtos
{
    public class ChartOfAccountUpdateDto
    {
        public string AccountName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
