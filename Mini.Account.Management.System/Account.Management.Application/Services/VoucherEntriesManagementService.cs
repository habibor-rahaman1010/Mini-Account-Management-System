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

        public async Task CreateVoucherEntry(string action, VoucherEntry voucherEntry)
        {
            await _voucherEntriesRepository.CreateAsync(action, voucherEntry);
        }

        public Task<(IList<VoucherEntry> voucherEntries, int totalCount)> GetVoucherEntries(string action, int pageNumber, int pageSize)
        {
            return _voucherEntriesRepository.GetAllAsync(action, pageNumber, pageSize);
        }

        public async Task<VoucherEntry> GetVoucherEntryById(string action, Guid id)
        {
            return await _voucherEntriesRepository.GetByIdAsync(action, id);
        }

        public async Task UpdateVoucherEntry(string action, Guid id, VoucherEntry voucherEntry)
        {
            await _voucherEntriesRepository.UpdateAsync(action, id, voucherEntry);
        }

        public async Task DeleteVoucherEntry(string action, Guid id)
        {
            await _voucherEntriesRepository.DeleteAsync(action, id);
        }

        public async Task<int> GetTotalVoucherEntriesCountAsync(string action)
        {
            return await _voucherEntriesRepository.GetTotalCountAsync(action);
        }
    }
}
