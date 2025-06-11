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

        public async Task CreateAsync(string action, VoucherEntry entry)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_ManageVoucherEntries", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "CREATE");
                    cmd.Parameters.AddWithValue("@VoucherTypeId", entry.VoucherTypeId);
                    cmd.Parameters.AddWithValue("@AccountId", entry.AccountId);
                    cmd.Parameters.AddWithValue("@DebitAmount", entry.DebitAmount);
                    cmd.Parameters.AddWithValue("@ReferenceNo", entry.ReferenceNo);
                    cmd.Parameters.AddWithValue("@CreditAmount", entry.CreditAmount);
                    cmd.Parameters.AddWithValue("@Narration", entry.Narration ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedDate", entry.CreatedDate == default ? (object)DBNull.Value : entry.CreatedDate);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }              
        }
    }
}
