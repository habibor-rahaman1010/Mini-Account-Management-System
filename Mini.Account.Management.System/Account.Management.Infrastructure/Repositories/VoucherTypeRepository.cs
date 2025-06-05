using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Account.Management.Infrastructure.Repositories
{
    public class VoucherTypeRepository : IVoucherTypeRepository
    {
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;

        public VoucherTypeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(string action, VoucherType voucherType)
        {
            if (voucherType == null) throw new ArgumentNullException(nameof(voucherType));

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_Manage_VoucherTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue(@"Action", action);
                    command.Parameters.AddWithValue("@Id", voucherType.Id);
                    command.Parameters.AddWithValue("@TypeName", voucherType.TypeName);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                } ;              
            }
        }

        public async Task<(IList<VoucherType>, int)> GetAllAsync(string action, int pageNumber, int pageSize)
        {
            var voucherTypeList = new List<VoucherType>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_Manage_VoucherTypes", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
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
                        voucherTypeList.Add(new VoucherType
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                        });
                    }
                    await reader.CloseAsync();
                    totalCount = (int)totalCountParam.Value;
                }
            }
            return (voucherTypeList, totalCount);
        }

        public async Task<VoucherType> GetByIdAsync(string action, Guid id)
        {
            VoucherType voucherType = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_Manage_VoucherTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(@"Action", action);
                    command.Parameters.AddWithValue(@"Id", id);
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            voucherType = new VoucherType
                            {
                                Id = reader.GetGuid("Id"),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                            };
                        }
                    }
                    await connection.CloseAsync();
                }
            }
            return voucherType;
        }

        public async Task UpdateAsync(string action, Guid id, VoucherType voucherType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_Manage_VoucherTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(@"Action", action);
                    command.Parameters.AddWithValue(@"Id", id);
                    command.Parameters.AddWithValue(@"TypeName", voucherType.TypeName ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(string action, Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_Manage_VoucherTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
