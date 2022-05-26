using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        public Task<BankAccount> GetBankAccount(string id);
        public Task CreateBankAccount(BankAccount bankAccount);
        public Task<List<BankAccount>> GetAllBankAccounts();
        public Task UpdateBankAccount(BankAccount bankAccount);
        public Task LockBankAccount(string bankAccountId);
        public Task<bool> IsLinkedBankAccount(string userId);
    }
}