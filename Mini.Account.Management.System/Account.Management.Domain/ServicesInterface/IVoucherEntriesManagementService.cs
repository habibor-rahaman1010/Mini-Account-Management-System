using Account.Management.Domain.Entities;

namespace Account.Management.Domain.ServicesInterface
{
    public interface IVoucherEntriesManagementService
    {
        public Task CreateVoucherEntry(string action, VoucherEntry voucherEntry);
        public Task<(IList<VoucherEntry> voucherEntries, int totalCount)> GetVoucherEntries(string action, int pageNumber, int pageSize);       
        public Task UpdateVoucherEntry(string action, Guid id, VoucherEntry voucherEntry);
        public Task<VoucherEntry> GetVoucherEntryById(string action, Guid id);
        public Task DeleteVoucherEntry(string action, Guid id);
        public Task<int> GetTotalVoucherEntriesCountAsync(string action);
    }
}
