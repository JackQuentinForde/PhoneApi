using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(
            IConfiguration configuration
            )
        {
            _connectionString = configuration.GetConnectionString("PhoneDatabase");
        }

        public async Task CreateAccount(string name)
        {
            string createAccountSql = @$"
                INSERT INTO 
                    Account(Name, Active)
                VALUES (
                    '{name}', 1);";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = createAccountSql;
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
