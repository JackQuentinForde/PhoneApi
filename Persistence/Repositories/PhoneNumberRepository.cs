using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repositories
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        private readonly string _connectionString;

        public PhoneNumberRepository(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("PhoneDatabase");
        }

        public async Task CreatePhoneNumber(string phoneNumber, int accountId)
        {
            string createPhoneNumberSql = @$"
                INSERT INTO 
                    PhoneNumber(Number, AccountId)
                VALUES (
                    '{phoneNumber}', {accountId});";
            await ExecuteSqlCommand(createPhoneNumberSql);
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
