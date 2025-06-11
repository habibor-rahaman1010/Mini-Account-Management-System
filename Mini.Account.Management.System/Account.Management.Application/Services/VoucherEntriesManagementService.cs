using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Application.Services
{
    public class VoucherEntriesManagementService : IVoucherEntriesManagementService
    {
        private readonly IVoucherEntriesRepository _voucherEntriesRepository;

        public VoucherEntriesManagementService(IVoucherEntriesRepository voucherEntriesRepository)
        {
            _voucherEntriesRepository = voucherEntriesRepository;
        }

        public async Task CreateVoucher(string action, VoucherEntry voucher)
        {
            await _voucherEntriesRepository.CreateAsync(action, voucher);
        }
    }
}
