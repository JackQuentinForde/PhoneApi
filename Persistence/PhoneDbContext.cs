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
        public virtual DbSet<PhoneNumber> PhoneNumber { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildAccounts(modelBuilder);
            BuildPhoneNumber(modelBuilder);
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

        private void BuildPhoneNumber(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneNumber>(e =>
            {
                e.ToTable("PhoneNumber");
                e.HasKey(c => c.Id);
                e.Property(c => c.Number).HasMaxLength(11);
                e.Property(c => c.AccountId);
            });
        }
    }
}
