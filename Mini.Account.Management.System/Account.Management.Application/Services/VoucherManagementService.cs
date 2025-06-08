using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;

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

        public Task<(IList<Voucher> voucherTypes, int totalCount)> GetVouchers(string action, int pageNumber, int pageSize)
        {
            return _voucherRepository.GetAllAsync(action, pageNumber, pageSize);
        }
    }
}
