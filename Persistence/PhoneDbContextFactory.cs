using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace Persistence
{
    [ExcludeFromCodeCoverage]
    public class PhoneDbContextFactory : IDesignTimeDbContextFactory<PhoneDbContext>
    {
        public PhoneDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PhoneDbContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=PhoneDatabase; Integrated Security=True; TrustServerCertificate=True;");
            return new PhoneDbContext(optionsBuilder.Options);
        }
    }
}