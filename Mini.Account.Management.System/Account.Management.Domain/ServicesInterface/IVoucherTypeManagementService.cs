using Account.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.ServicesInterface
{
    public interface IVoucherTypeManagementService
    {
        public Task AddVoucherType(string action, VoucherType voucherType);
        public Task<(IList<VoucherType> voucherTypes, int totalCount)> GetVoucherTypes(string action, int pageNumber, int pageSize);
    }
}
