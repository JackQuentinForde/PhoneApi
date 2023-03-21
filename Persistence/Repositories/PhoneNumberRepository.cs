using Domain.Entities;
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

        public async Task DeletePhoneNumber(int Id)
        {
            string deletePhoneNumberSql = @$"
                DELETE FROM
                    PhoneNumber
                WHERE
                    Id = '{Id}';";
            await ExecuteSqlCommand(deletePhoneNumberSql);
        }

        public async Task<PhoneNumber?> GetPhoneNumber(int id)
        {
            string getPhoneNumberSql = @$"
                SELECT
                    Id,
                    Number,
                    AccountId
                FROM
                    PhoneNumber
                WHERE
                    Id = '{id}'";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = getPhoneNumberSql;
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                    return new PhoneNumber(
                        reader.GetInt32("Id"),
                        reader.GetString("Number"),
                        reader.GetInt32("AccountId"));
                return null;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IReadOnlyCollection<PhoneNumber>> GetAll(int accountId)
        {
            string getAllSql = @$"
                SELECT
                    Id,
                    Number,
                    AccountId
                FROM
                    PhoneNumber
                WHERE
                    AccountId = '{accountId}'";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = getAllSql;
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                var phoneNumbers = new List<PhoneNumber>();
                while (reader.Read())
                    phoneNumbers.Add(new PhoneNumber(
                        reader.GetInt32("Id"),
                        reader.GetString("Number"),
                        reader.GetInt32("AccountId")));
                return phoneNumbers.AsReadOnly();
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
