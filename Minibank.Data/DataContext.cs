using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Minibank.Data.BankAccounts;
using Minibank.Data.MoneyTransfers;
using Minibank.Data.Users;

namespace Minibank.Data
{
    public class DataContext: DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<BankAccountDbModel> BankAccounts { get; set; }
        public DbSet<MoneyTransferDbModel> MoneyTransfer { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }

    public class Factory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Port=5432;Database=MiniBankDB;Username=postgres;Password=mandarin2012")
                .Options;

            return new DataContext(options);
        }
    }
}