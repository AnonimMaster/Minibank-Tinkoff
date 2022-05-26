﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minibank.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Minibank.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Minibank.Data.BankAccounts.BankAccountDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("closing_date");

                    b.Property<int>("CurrencyCode")
                        .HasColumnType("integer")
                        .HasColumnName("currency_code");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_locked");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("opening_date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("bank_account");
                });

            modelBuilder.Entity("Minibank.Data.MoneyTransfers.MoneyTransferDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<int>("CurrencyCode")
                        .HasColumnType("integer")
                        .HasColumnName("currency_code");

                    b.Property<string>("FromAccountId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("from_account_id");

                    b.Property<string>("ToAccountId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("to_account_id");

                    b.HasKey("Id")
                        .HasName("pk_id");

                    b.HasIndex("FromAccountId");

                    b.HasIndex("ToAccountId");

                    b.ToTable("money_transfer");
                });

            modelBuilder.Entity("Minibank.Data.Users.UserDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Minibank.Data.BankAccounts.BankAccountDbModel", b =>
                {
                    b.HasOne("Minibank.Data.Users.UserDbModel", "User")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Minibank.Data.MoneyTransfers.MoneyTransferDbModel", b =>
                {
                    b.HasOne("Minibank.Data.BankAccounts.BankAccountDbModel", "FromAccount")
                        .WithMany("SentMoneyTransfers")
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Minibank.Data.BankAccounts.BankAccountDbModel", "ToAccount")
                        .WithMany("AcceptMoneyTransfers")
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromAccount");

                    b.Navigation("ToAccount");
                });

            modelBuilder.Entity("Minibank.Data.BankAccounts.BankAccountDbModel", b =>
                {
                    b.Navigation("AcceptMoneyTransfers");

                    b.Navigation("SentMoneyTransfers");
                });

            modelBuilder.Entity("Minibank.Data.Users.UserDbModel", b =>
                {
                    b.Navigation("BankAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
