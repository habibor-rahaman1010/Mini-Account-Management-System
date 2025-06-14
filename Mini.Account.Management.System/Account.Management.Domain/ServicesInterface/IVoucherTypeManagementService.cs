using Account.Management.Domain.Entities;

namespace Account.Management.Domain.ServicesInterface
{
    public interface IVoucherTypeManagementService
    {
        public Task AddVoucherType(string action, VoucherType voucherType);
        public Task<(IList<VoucherType> voucherTypes, int totalCount)> GetVoucherTypes(string action, int pageNumber, int pageSize);
        public Task UpdateVoucherType(string action, Guid id, VoucherType voucherType);
        public Task<VoucherType> GetVoucherTypeById(string action, Guid id);
        public Task DeleteVoucherType(string action, Guid id);
        public Task<int> GetTotalVoucherTypesCountAsync(string action);
    }
}
