using Account.Management.Domain.Dtos;
using Account.Management.Domain.Entities;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IChartOfAccountRepository
    {
        public Task CreateAsync(string action, ChartOfAccount account);
        public Task<ChartOfAccountDto> GetById(Guid id);
        public Task<(IList<ChartOfAccountDto> accounts, int totalCount)> GetChartOfAccountsAsync(int pageNumber, int pageSize);
        public Task UpdateAsync(Guid id, ChartOfAccountUpdateDto account);
        public Task DeleteAsync(string action, Guid id);
    }
}