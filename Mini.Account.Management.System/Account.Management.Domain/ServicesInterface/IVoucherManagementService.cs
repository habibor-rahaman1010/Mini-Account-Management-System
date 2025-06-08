using Account.Management.Domain.Entities;


namespace Account.Management.Domain.ServicesInterface
{
    public interface IVoucherManagementService
    {
        public Task CreateVoucher(string action, Voucher voucher);
        public Task<(IList<Voucher> voucherTypes, int totalCount)> GetVouchers(string action, int pageNumber, int pageSize);
    }
}
