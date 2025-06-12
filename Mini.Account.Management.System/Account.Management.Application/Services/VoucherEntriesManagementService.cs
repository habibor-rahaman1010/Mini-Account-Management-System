using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;

namespace Account.Management.Application.Services
{
    public class VoucherEntriesManagementService : IVoucherEntriesManagementService
    {
        private readonly IVoucherEntriesRepository _voucherEntriesRepository;

        public VoucherEntriesManagementService(IVoucherEntriesRepository voucherEntriesRepository)
        {
            _voucherEntriesRepository = voucherEntriesRepository;
        }

        public async Task CreateVoucherEntry(string action, VoucherEntry voucher)
        {
            await _voucherEntriesRepository.CreateAsync(action, voucher);
        }

        public Task<(IList<VoucherEntry> voucherEntries, int totalCount)> GetVoucherEntries(string action, int pageNumber, int pageSize)
        {
            return _voucherEntriesRepository.GetAllAsync(action, pageNumber, pageSize);
        }
    }
}
