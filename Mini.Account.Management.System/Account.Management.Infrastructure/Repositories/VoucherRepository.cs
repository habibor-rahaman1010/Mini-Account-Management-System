using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Account.Management.Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;

        public VoucherRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(string action, Voucher voucher)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVouchers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", voucher.Id);
                    command.Parameters.AddWithValue("@VoucherDate", voucher.VoucherDate);
                    command.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo);
                    command.Parameters.AddWithValue("@VoucherTypeId", voucher.VoucherTypeId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
