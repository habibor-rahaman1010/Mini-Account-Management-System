using Account.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Management.Domain.RepositoriesInterface
{
    public interface IVoucherTypeRepository
    {
        public Task CreateAsync(string action, VoucherType voucherType);
        public Task<(IList<VoucherType>, int)> GetAllAsync(string action, int pageNumber, int pageSize);
        public Task UpdateAsync(string action, Guid id, VoucherType voucherType);
        public Task<VoucherType> GetByIdAsync(string action, Guid id);
        public Task DeleteAsync(string action, Guid id);
    }
}
