using Account.Management.Domain.Dtos;
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

        public async Task<ChartOfAccountDto> GetById(Guid id)
        {
            ChartOfAccountDto account = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ChartOfAccounts WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            account = new ChartOfAccountDto
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
                    }
                }
            }
            return account;
        }

        public async Task<(IList<ChartOfAccountDto> accounts, int totalCount)> GetChartOfAccountsAsync(int pageNumber, int pageSize)
        {
            var list = new List<ChartOfAccountDto>();

            string countQuery = "SELECT COUNT(*) FROM ChartOfAccounts";
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var countCommand = new SqlCommand(countQuery, conn);
            int totalCount = (int)await countCommand.ExecuteScalarAsync();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM ChartOfAccounts ORDER BY AccountName OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var command = new SqlCommand(query, connection);

                // Calculate offset based on page number and size
                int offset = (pageNumber - 1) * pageSize;
                command.Parameters.AddWithValue("@Offset", offset);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new ChartOfAccountDto
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            AccountName = reader.GetString(reader.GetOrdinal("AccountName")),
                            Code = reader.GetString(reader.GetOrdinal("Code")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            AccountType = reader.GetString(reader.GetOrdinal("AccountType")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            ParentId = reader["ParentId"] == DBNull.Value ? null : reader.GetGuid(reader.GetOrdinal("ParentId")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : (DateTime?)reader["ModifiedDate"]
                        });
                    }
                }
            }

            return (list, totalCount);
        }

        public async Task UpdateAsync(Guid id, ChartOfAccountUpdateDto account)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ManageChartOfAccounts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Action", "UPDATE");
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
    }
}
