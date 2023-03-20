using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Persistence
{
    [ExcludeFromCodeCoverage]
    public class PhoneDbContext : DbContext
    {
        public PhoneDbContext()
        {
        }

        public PhoneDbContext(DbContextOptions<PhoneDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildAccounts(modelBuilder);
        }

        private void BuildAccounts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Account");
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).HasMaxLength(120);
                e.Property(c => c.Active);
            });
        }
    }
}
