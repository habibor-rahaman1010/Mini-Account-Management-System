using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;

namespace Account.Management.Application.Services
{
    public class ChartOfAccountManagementService : IChartOfAccountManagementService
    {
        private readonly IChartOfAccountRepository _chartOfAccountRepository;

        public ChartOfAccountManagementService(IChartOfAccountRepository chartOfAccountRepository)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
        }

        public async Task<ChartOfAccount> ChartOfAccountById(string action, Guid id)
        {
            return await _chartOfAccountRepository.GetById(action, id);
        }

        public async Task CreateChatOfAccount(string action, ChartOfAccount chartOfAccount)
        {
            await _chartOfAccountRepository.CreateAsync(action, chartOfAccount);
        }

        public async Task DeleteChartOfAccount(string action, Guid id)
        {
            await _chartOfAccountRepository.DeleteAsync(action, id);
        }

        public async Task<(IList<ChartOfAccount> chatOfAccounts, int totalCount)> GetchatOfAccounts(string action, int pageNumber, int pageSize)
        {
            return await _chartOfAccountRepository.GetChartOfAccountsAsync(action, pageNumber, pageSize);
        }

        public async Task<int> GetTotalChartOfAccountsCountAsync(string action)
        {
            return await _chartOfAccountRepository.GetTotalCountAsync(action);
        }

        public async Task UpdateChatOfAccount(string action, Guid id, ChartOfAccount chartOfAccount)
        {
            await _chartOfAccountRepository.UpdateAsync(action, id, chartOfAccount);
        }
    }
}
