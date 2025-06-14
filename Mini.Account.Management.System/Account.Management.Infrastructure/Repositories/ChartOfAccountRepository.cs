using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Account.Management.Infrastructure.Repositories
{
    public class ChartOfAccountRepository : IChartOfAccountRepository
    {
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;

        public ChartOfAccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(string action, ChartOfAccount account)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("sp_ManageChartOfAccounts", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Action", action);
                command.Parameters.AddWithValue("@Id", account.Id);
                command.Parameters.AddWithValue("@AccountName", account.AccountName);
                command.Parameters.AddWithValue("@Code", account.Code);
                command.Parameters.AddWithValue("@Description", account.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@AccountType", account.AccountType);
                command.Parameters.AddWithValue("@IsActive", account.IsActive);
                command.Parameters.AddWithValue("@ParentId", account.ParentId ?? (object)DBNull.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<ChartOfAccount> GetById(string action, Guid id)
        {
            ChartOfAccount chartOfAccount = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ManageChartOfAccounts", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            chartOfAccount = new ChartOfAccount
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                                Code = reader.GetString(reader.GetOrdinal("Code")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                            };
                        }
                        await reader.CloseAsync();
                    }
                }
            }
            return chartOfAccount;
        }

        public async Task<(IList<ChartOfAccount> chatOfAccounts, int totalCount)> GetChartOfAccountsAsync(string action, int pageNumber, int pageSize)
        {
            var chartOfAccounts = new List<ChartOfAccount>();
            var totalCount = 0;
            
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageChartOfAccounts", connection))
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

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            chartOfAccounts.Add(new ChartOfAccount
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                                Code = reader.GetString(reader.GetOrdinal("Code")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId"))? (Guid?)null: reader.GetGuid(reader.GetOrdinal("ParentId")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                            });
                        }
                        await reader.CloseAsync();
                        totalCount = (int)totalCountParam.Value;
                    }
                }
            }
            return (chartOfAccounts, totalCount);
        }

        public async Task UpdateAsync(string action, Guid id, ChartOfAccount account)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ManageChartOfAccounts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@AccountName", account.AccountName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Code", account.Code ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", account.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountType", account.AccountType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", account.IsActive);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(string action, Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ManageChartOfAccounts", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> GetTotalCountAsync(string action)
        {
            int totalCount = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sp_ManageChartOfAccounts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", action);

                    var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
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
