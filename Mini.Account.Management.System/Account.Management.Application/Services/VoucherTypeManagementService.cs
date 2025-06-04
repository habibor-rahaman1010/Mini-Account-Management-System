using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;

namespace Account.Management.Application.Services
{
    public class VoucherTypeManagementService : IVoucherTypeManagementService
    {
        private readonly IVoucherTypeRepository _voucherTypeRepository;
        public VoucherTypeManagementService(IVoucherTypeRepository voucherTypeRepository)
        {
            _voucherTypeRepository = voucherTypeRepository; 
        }

        public async Task AddVoucherType(string action, VoucherType voucherType)
        {
            await _voucherTypeRepository.CreateAsync(action, voucherType);
        }

        public async Task<(IList<VoucherType>, int)> GetVoucherTypes(string action, int pageNumber, int pageSize)
        {
            return await _voucherTypeRepository.GetAllAsync(action, pageNumber, pageSize);
        }
    }
}
