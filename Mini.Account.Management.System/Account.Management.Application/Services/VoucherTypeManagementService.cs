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

        public async Task<VoucherType> GetVoucherTypeById(string action, Guid id)
        {
            return await _voucherTypeRepository.GetByIdAsync(action, id);
        }

        public async Task<(IList<VoucherType>, int)> GetVoucherTypes(string action, int pageNumber, int pageSize)
        {
            return await _voucherTypeRepository.GetAllAsync(action, pageNumber, pageSize);
        }

        public async Task UpdateVoucherType(string action, Guid id, VoucherType voucherType)
        {
            await _voucherTypeRepository.UpdateAsync(action, id, voucherType);
        }

        public async Task DeleteVoucherType(string action, Guid id)
        {
            await _voucherTypeRepository.DeleteAsync(action, id);
        }

        public async Task<int> GetTotalVoucherTypesCountAsync(string action)
        {
            return await _voucherTypeRepository.GetTotalCountAsync(action);
        }
    }
}
