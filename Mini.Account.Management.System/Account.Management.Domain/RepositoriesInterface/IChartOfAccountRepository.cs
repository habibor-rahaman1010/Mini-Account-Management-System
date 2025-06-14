using Account.Management.Domain.Entities;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IChartOfAccountRepository
    {
        public Task CreateAsync(string action, ChartOfAccount account);
        public Task<ChartOfAccount> GetById(string action, Guid id);
        public Task<(IList<ChartOfAccount> chatOfAccounts, int totalCount)> GetChartOfAccountsAsync(string action, int pageNumber, int pageSize);
        public Task UpdateAsync(string action, Guid id, ChartOfAccount account);
        public Task DeleteAsync(string action, Guid id);
        public Task<int> GetTotalCountAsync(string action);
    }
}