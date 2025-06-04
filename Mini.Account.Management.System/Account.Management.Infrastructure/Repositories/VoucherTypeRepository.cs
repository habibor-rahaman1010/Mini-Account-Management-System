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
    }
}
