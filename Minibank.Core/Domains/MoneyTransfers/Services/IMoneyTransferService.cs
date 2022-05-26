using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public interface IMoneyTransferService
    {
        public Task<List<MoneyTransfer>> GetAllTransfers();
        public Task<List<MoneyTransfer>> GetAllTransfersBankAccount(string bankAccountId);
        public Task<double> CalculateCommission(double amount, string fromBankAccountId, string toBankAccountId);
        public Task TransferMoney(double amount, string fromBankAccountId, string toBankAccountId);
    }
}