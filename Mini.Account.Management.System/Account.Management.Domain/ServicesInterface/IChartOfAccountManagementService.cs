using Account.Management.Domain.Entities;


namespace Account.Management.Domain.ServicesInterface
{
    public interface IChartOfAccountManagementService
    {
        public Task CreateChatOfAccount(string action, ChartOfAccount chatOfAccount);
        public Task<(IList<ChartOfAccount> chatOfAccounts, int totalCount)> GetchatOfAccounts(string action, int pageNumber, int pageSize);       
        public Task UpdateChatOfAccount(string action, Guid id, ChartOfAccount chatOfAccount);
        public Task<ChartOfAccount> ChartOfAccountById(string action, Guid id);
        public Task DeleteChartOfAccount(string action, Guid id);
        public Task<int> GetTotalChartOfAccountsCountAsync(string action);
    }
}
