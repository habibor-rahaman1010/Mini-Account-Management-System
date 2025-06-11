using Account.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IVoucherEntriesRepository
    {

        public Task CreateAsync(string action, VoucherEntry voucher);

        /*public Task<(IList<Voucher>, int)> GetAllAsync(string action, int pageNumber, int pageSize);
        public Task UpdateAsync(string action, Guid id, Voucher voucher);
        public Task<Voucher> GetByIdAsync(string action, Guid id);
        public Task DeleteAsync(string action, Guid id);*/
        
    }
}
