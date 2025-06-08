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
    public class VoucherManagementService : IVoucherManagementService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherManagementService(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task CreateVoucher(string action, Voucher voucher)
        {
            await _voucherRepository.CreateAsync(action, voucher);
        }
    }
}
