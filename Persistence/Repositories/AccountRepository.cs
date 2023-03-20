using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
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
            await ExecuteSqlCommand(createAccountSql);
        }

        public async Task SetActive(int id, bool active)
        {
            string setActiveSql = @$"
                UPDATE 
                    Account
                SET
                    Active = {Convert.ToByte(active)}
                WHERE
                    Id = {id}
            ";
            await ExecuteSqlCommand(setActiveSql);
        }

        public async Task<bool> IsActive(int id)
        {
            string getActiveSql = @$"
                SELECT
                    Active
                FROM
                    Account
                WHERE
                    Id = {id}
            ";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = getActiveSql;
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    return reader.GetBoolean("Active");
                return false;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task ExecuteSqlCommand(string sqlCommand)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
