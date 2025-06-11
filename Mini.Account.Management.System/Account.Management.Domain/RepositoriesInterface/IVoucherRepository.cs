using Account.Management.Domain.Entities;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IVoucherRepository
    {
        public Task CreateAsync(string action, Voucher voucher);
        public Task<(IList<Voucher>, int)> GetAllAsync(string action, int pageNumber, int pageSize);
        public Task UpdateAsync(string action, Guid id, Voucher voucher);
        public Task<Voucher> GetByIdAsync(string action, Guid id);
        public Task DeleteAsync(string action, Guid id);
    }
}
