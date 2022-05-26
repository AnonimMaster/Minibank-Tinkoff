using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.CurrencyConvert;
using Minibank.Data.BankAccounts;

namespace Minibank.Data.MoneyTransfers
{
    public class MoneyTransferDbModel
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string FromAccountId { get; set; }
        public BankAccountDbModel FromAccount { get; set; }
        public string ToAccountId { get; set; }
        public BankAccountDbModel ToAccount { get; set; }

        internal class Map : IEntityTypeConfiguration<MoneyTransferDbModel>
        {
            public void Configure(EntityTypeBuilder<MoneyTransferDbModel> builder)
            {
                builder.ToTable("money_transfer");

                builder.Property(it => it.Id)
                    .HasColumnName("id");
                builder.Property(it => it.Amount)
                    .HasColumnName("amount");
                builder.Property(it => it.CurrencyCode)
                    .HasColumnName("currency_code");
                builder.Property(it => it.FromAccountId)
                    .HasColumnName("from_account_id").IsRequired(true);
                builder.Property(it => it.ToAccountId)
                    .HasColumnName("to_account_id").IsRequired(true);

                builder.HasKey(it => it.Id).HasName("pk_id");
            }
        }
    }
}