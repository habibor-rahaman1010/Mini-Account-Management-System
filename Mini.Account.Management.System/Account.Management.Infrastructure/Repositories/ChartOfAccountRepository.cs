using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
                command.Parameters.AddWithValue("@CreatedDate", account.CreatedDate);
                command.Parameters.AddWithValue("@ModifiedDate", account.ModifiedDate);

                await connection.OpenAsync();
                command.ExecuteNonQuery();
            }
        }
    }
}
