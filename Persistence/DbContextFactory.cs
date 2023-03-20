using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public sealed class DbContextFactory
    {
        public static T Create<T>(string connectionString, bool onAzure) where T : DbContext
        {
            var sqlConnection = new SqlConnection(connectionString);
            if (onAzure)
            {
                var credential = new DefaultAzureCredential();
                var accessToken = credential.GetToken(new TokenRequestContext(new[] { "https://database.windows.net/" }));
                sqlConnection.AccessToken = accessToken.Token;
            }

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(sqlConnection, o =>
            {
                o.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
            });

            return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options);
        }
    }
}
