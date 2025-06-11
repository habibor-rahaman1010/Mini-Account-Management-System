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

        public async Task<(IList<Voucher>, int)> GetAllAsync(string action, int pageNumber, int pageSize)
        {
            var voucherList = new List<Voucher>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVouchers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(@"Action", action);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalCountParam);

                    await connection.OpenAsync();

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        voucherList.Add(new Voucher
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            VoucherDate = reader.GetDateTime(reader.GetOrdinal("VoucherDate")),
                            ReferenceNo = reader.GetString(reader.GetOrdinal("ReferenceNo")),
                            VoucherTypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                        });
                    }
                    await reader.CloseAsync();
                    totalCount = (int) totalCountParam.Value;
                }
            }
            return (voucherList, totalCount);
        }

        public async Task<Voucher> GetByIdAsync(string action, Guid id)
        {
            Voucher voucher = null;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_ManageVouchers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Action", action);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        voucher = new Voucher
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            VoucherDate = reader.GetDateTime(reader.GetOrdinal("VoucherDate")),
                            ReferenceNo = reader.GetString(reader.GetOrdinal("ReferenceNo")),
                            VoucherTypeId = reader.GetGuid(reader.GetOrdinal("VoucherTypeId")),
                            VoucherTypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                        };
                    }
                }

                await connection.CloseAsync();
            }

            return voucher;
        }

        public async Task UpdateAsync(string action, Guid id, Voucher voucher)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("sp_ManageVouchers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add required parameters
                command.Parameters.AddWithValue("@Action", action);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@VoucherDate", voucher.VoucherDate);
                command.Parameters.AddWithValue("@ReferenceNo", string.IsNullOrEmpty(voucher.ReferenceNo) ? DBNull.Value : voucher.ReferenceNo);
                command.Parameters.AddWithValue("@VoucherTypeId", voucher.VoucherTypeId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }
    }
}
