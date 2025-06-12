using Account.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.ServicesInterface
{
    public interface IVoucherEntriesManagementService
    {
        public Task CreateVoucherEntry(string action, VoucherEntry voucher);
        public Task<(IList<VoucherEntry> voucherEntries, int totalCount)> GetVoucherEntries(string action, int pageNumber, int pageSize);
        /* 
        public Task UpdateVoucher(string action, Guid id, Voucher voucher);
        public Task<Voucher> GetVoucherById(string action, Guid id);
        public Task DeleteVoucher(string action, Guid id);*/
    }
}
