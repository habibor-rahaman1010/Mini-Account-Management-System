using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
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

                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@VoucherTypeId", voucherEntry.VoucherTypeId);
                    command.Parameters.AddWithValue("@AccountId", voucherEntry.AccountId);
                    command.Parameters.AddWithValue("@DebitAmount", voucherEntry.DebitAmount);
                    command.Parameters.AddWithValue("@ReferenceNo", voucherEntry.ReferenceNo);
                    command.Parameters.AddWithValue("@CreditAmount", voucherEntry.CreditAmount);
                    command.Parameters.AddWithValue("@Narration", voucherEntry.Narration ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedDate", voucherEntry.CreatedDate == default ? (object)DBNull.Value : voucherEntry.CreatedDate);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
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

        public async Task<VoucherEntry> GetByIdAsync(string action, Guid id)
        {
            VoucherEntry voucherEntry = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            voucherEntry = new VoucherEntry
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                DebitAmount = reader.GetDecimal(reader.GetOrdinal("DebitAmount")),
                                CreditAmount = reader.GetDecimal(reader.GetOrdinal("CreditAmount")),
                                Narration = reader.GetString(reader.GetOrdinal("Narration")),
                                ReferenceNo = reader.GetString(reader.GetOrdinal("ReferenceNo")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                                AccountId = reader.GetGuid(reader.GetOrdinal("AccountId")),
                                AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                                VoucherTypeId = reader.GetGuid(reader.GetOrdinal("VoucherTypeId")),
                                VoucherTypeName = reader.GetString(reader.GetOrdinal("VoucherTypeName")),
                            };
                        }
                    }
                    await connection.CloseAsync();
                }
            }
            return voucherEntry;
        }

        public async Task UpdateAsync(string action, Guid id, VoucherEntry voucherEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);

                    command.Parameters.AddWithValue("@VoucherTypeId", voucherEntry.VoucherTypeId);
                    command.Parameters.AddWithValue("@AccountId", voucherEntry.AccountId);
                    command.Parameters.AddWithValue("@DebitAmount", voucherEntry.DebitAmount);
                    command.Parameters.AddWithValue("@ReferenceNo", voucherEntry.ReferenceNo);
                    command.Parameters.AddWithValue("@CreditAmount", voucherEntry.CreditAmount);
                    command.Parameters.AddWithValue("@Narration", voucherEntry.Narration ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedDate", voucherEntry.ModifiedDate == default ? (object)DBNull.Value : voucherEntry.ModifiedDate);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                }
            }
        }

        public async Task DeleteAsync(string action, Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                }
            }
        }

        public async Task<int> GetTotalCountAsync(string action)
        {
            var totalCount = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageVoucherEntries", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);

                    var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(totalCountParam);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    totalCount = (int)(totalCountParam.Value ?? 0);
                }
            }
            return totalCount;
        }
    }
}
