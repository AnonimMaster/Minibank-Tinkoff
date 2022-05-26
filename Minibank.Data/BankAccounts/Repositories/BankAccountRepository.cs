using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository: IBankAccountRepository
    {
        private readonly DataContext _context;

        public BankAccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetBankAccount(string id)
        {
            var entityBankAccount = await _context.BankAccounts.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);


            if (entityBankAccount == null)
            {
                throw new ObjectNotFoundException($"Аккаунт с Id ={id} не существует.");
            }

            return new BankAccount()
            {
                Id = entityBankAccount.Id,
                UserId = entityBankAccount.UserId,
                Amount = entityBankAccount.Amount,
                CurrencyCode = entityBankAccount.CurrencyCode,
                IsLocked = entityBankAccount.IsLocked,
                OpeningDate = entityBankAccount.OpeningDate,
                ClosingDate = entityBankAccount.ClosingDate
            };
        }

        public Task CreateBankAccount(BankAccount bankAccount)
        {
            var entityBankAccount = new BankAccountDbModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = bankAccount.UserId,
                Amount = bankAccount.Amount,
                CurrencyCode = bankAccount.CurrencyCode,
                IsLocked = bankAccount.IsLocked,
                OpeningDate = bankAccount.OpeningDate,
                ClosingDate = bankAccount.ClosingDate
            };

            return _context.BankAccounts.AddAsync(entityBankAccount).AsTask();
        }

        public Task<List<BankAccount>> GetAllBankAccounts()
        {
            return _context.BankAccounts.Select(i => new BankAccount()
            {
                Id = i.Id,
                UserId = i.UserId,
                Amount = i.Amount,
                CurrencyCode = i.CurrencyCode,
                IsLocked = i.IsLocked,
                OpeningDate = i.OpeningDate,
                ClosingDate = i.ClosingDate
            }).AsNoTracking().ToListAsync();
        }

        public async Task UpdateBankAccount(BankAccount bankAccount)
        {
            var entityBankAccount = await _context.BankAccounts.FirstOrDefaultAsync(i => i.Id == bankAccount.Id);

            if (entityBankAccount == null)
            {
                throw new ObjectNotFoundException($"Банковский аккаунт с Id = {bankAccount.Id} не существует.");
            }

            entityBankAccount.UserId = bankAccount.UserId;
            entityBankAccount.Amount = bankAccount.Amount;
            entityBankAccount.CurrencyCode = bankAccount.CurrencyCode;
            entityBankAccount.IsLocked = bankAccount.IsLocked;
            entityBankAccount.OpeningDate = bankAccount.OpeningDate;
            entityBankAccount.ClosingDate = bankAccount.ClosingDate;
        }

        public async Task LockBankAccount(string bankAccountId)
        {
            var entityBankAccount = await _context.BankAccounts.FirstOrDefaultAsync(i => i.Id == bankAccountId);

            if (entityBankAccount == null)
            {
                throw new ObjectNotFoundException($"Банковский аккаунт с Id = {bankAccountId} не существует.");
            }

            entityBankAccount.IsLocked = true;
            entityBankAccount.ClosingDate = DateTime.UtcNow;
        }

        public Task<bool> IsLinkedBankAccount(string userId)
        {
            return _context.BankAccounts.AsNoTracking().AnyAsync(i => i.UserId == userId & i.IsLocked == false);
        }
    }
}