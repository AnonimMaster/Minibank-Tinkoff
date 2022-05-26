using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.MoneyTransfers.Repositories
{
    public interface IMoneyTransferRepository
    {
        public Task<MoneyTransfer> GetTransfer(string id);
        public Task<List<MoneyTransfer>> GetAllTransfers();
        public Task Create(MoneyTransfer transfer);
    }
}