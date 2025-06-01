using Account.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IChartOfAccountRepository
    {
        public Task CreateAsync(string action, ChartOfAccount account);
    }
}
