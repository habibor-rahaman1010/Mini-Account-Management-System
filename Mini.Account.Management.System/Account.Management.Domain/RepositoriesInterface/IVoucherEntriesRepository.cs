using Account.Management.Domain.Entities;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IVoucherEntriesRepository
    {

        public Task CreateAsync(string action, VoucherEntry voucherEntry);
        public Task<(IList<VoucherEntry>, int)> GetAllAsync(string action, int pageNumber, int pageSize);  
        public Task<VoucherEntry> GetByIdAsync(string action, Guid id);
        public Task UpdateAsync(string action, Guid id, VoucherEntry voucherEntry);
        public Task DeleteAsync(string action, Guid id);

    }
}
