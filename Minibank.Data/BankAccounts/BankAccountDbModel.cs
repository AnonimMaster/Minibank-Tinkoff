using Minibank.Core.Domains.CurrencyConvert;
using System;
using System.Collections.Generic;
using Minibank.Data.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Minibank.Data.MoneyTransfers;

namespace Minibank.Data.BankAccounts
{
    public class BankAccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserDbModel User { get; set; }
        public double Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public bool IsLocked { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public List<MoneyTransferDbModel> SentMoneyTransfers { get; set; }
        public List<MoneyTransferDbModel> AcceptMoneyTransfers { get; set; }

        internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
        {
            public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
            {
                builder.ToTable("bank_account");

                builder.HasOne(it => it.User)
                    .WithMany(it => it.BankAccounts)
                    .HasForeignKey(it => it.UserId);

                builder.HasMany(it => it.SentMoneyTransfers).WithOne(it => it.FromAccount)
                    .HasForeignKey(it => it.FromAccountId);
                builder.HasMany(it => it.AcceptMoneyTransfers).WithOne(it => it.ToAccount)
                    .HasForeignKey(it => it.ToAccountId);

                builder.Property(it => it.Id).HasColumnName("id");
                builder.Property(it => it.UserId).IsRequired(true).HasColumnName("user_id");
                builder.Property(it => it.Amount).HasColumnName("amount");
                builder.Property(it => it.CurrencyCode).HasColumnName("currency_code");
                builder.Property(it => it.IsLocked).HasColumnName("is_locked");
                builder.Property(it => it.OpeningDate).HasColumnName("opening_date");
                builder.Property(it => it.ClosingDate).HasColumnName("closing_date");
            }
        }
    }
}