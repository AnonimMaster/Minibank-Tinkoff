using Minibank.Core.Domains.CurrencyConvert;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        public Task<BankAccount> GetBankAccount(string id);
        public Task CreateBankAccount(string userId, CurrencyCode currencyCode, int initialAmount);
        public Task<List<BankAccount>> GetAllBankAccounts();
        public Task LockBankAccount(string id);
    }
}