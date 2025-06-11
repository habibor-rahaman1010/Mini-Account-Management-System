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
        public Task CreateVoucher(string action, VoucherEntry voucher);
       /* public Task<(IList<Voucher> voucherTypes, int totalCount)> GetVouchers(string action, int pageNumber, int pageSize);
        public Task UpdateVoucher(string action, Guid id, Voucher voucher);
        public Task<Voucher> GetVoucherById(string action, Guid id);
        public Task DeleteVoucher(string action, Guid id);*/
    }
}
