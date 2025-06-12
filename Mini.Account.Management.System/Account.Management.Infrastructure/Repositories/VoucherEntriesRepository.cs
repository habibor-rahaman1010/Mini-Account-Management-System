using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Account.Management.Infrastructure.Repositories
{
    public class VoucherEntriesRepository : IVoucherEntriesRepository
    {
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;

        public VoucherEntriesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(string action, VoucherEntry voucherEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Action", "CREATE");
                    command.Parameters.AddWithValue("@VoucherTypeId", voucherEntry.VoucherTypeId);
                    command.Parameters.AddWithValue("@AccountId", voucherEntry.AccountId);
                    command.Parameters.AddWithValue("@DebitAmount", voucherEntry.DebitAmount);
                    command.Parameters.AddWithValue("@ReferenceNo", voucherEntry.ReferenceNo);
                    command.Parameters.AddWithValue("@CreditAmount", voucherEntry.CreditAmount);
                    command.Parameters.AddWithValue("@Narration", voucherEntry.Narration ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDate", voucherEntry.CreatedDate == default ? (object)DBNull.Value : voucherEntry.CreatedDate);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<(IList<VoucherEntry>, int)> GetAllAsync(string action, int pageNumber, int pageSize)
        {
            var voucherEntryList = new List<VoucherEntry>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
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
                        voucherEntryList.Add(new VoucherEntry
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            DebitAmount = reader.GetDecimal(reader.GetOrdinal("DebitAmount")),
                            CreditAmount = reader.GetDecimal(reader.GetOrdinal("CreditAmount")),
                            Narration = reader.GetString(reader.GetOrdinal("Narration")),
                            ReferenceNo = reader.GetString(reader.GetOrdinal("ReferenceNo")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                            AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                            VoucherTypeName = reader.GetString(reader.GetOrdinal("VoucherTypeName")),
                        });
                    }
                    await reader.CloseAsync();
                    totalCount = (int)totalCountParam.Value;
                }
            }
            return (voucherEntryList, totalCount);
        }
    }
}
