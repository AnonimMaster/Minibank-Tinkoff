using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Data.MoneyTransfers.Repositories
{
    public class MoneyTransferRepository: IMoneyTransferRepository
    {
        private readonly DataContext _context;

        public MoneyTransferRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<MoneyTransfer> GetTransfer(string id)
        {
            var entityMoneyTransfer = await _context.MoneyTransfer.FirstOrDefaultAsync(i => i.Id == id);

            if (entityMoneyTransfer == null)
            {
                throw new ObjectNotFoundException($"Денежного перевода с Id = {id} не существует");
            }

            return new MoneyTransfer()
            {
                Id = entityMoneyTransfer.Id,
                Amount = entityMoneyTransfer.Amount,
                CurrencyCode = entityMoneyTransfer.CurrencyCode,
                FromAccountId = entityMoneyTransfer.FromAccountId,
                ToAccountId = entityMoneyTransfer.ToAccountId
            };
        }

        public Task<List<MoneyTransfer>> GetAllTransfers()
        {
            return _context.MoneyTransfer.Select(i => new MoneyTransfer()
            {
                Id = i.Id,
                Amount = i.Amount,
                CurrencyCode = i.CurrencyCode,
                FromAccountId = i.FromAccountId,
                ToAccountId = i.ToAccountId
            }).AsNoTracking().ToListAsync();
        }

        public Task Create(MoneyTransfer transfer)
        {
            var entityMoneyTransfer = new MoneyTransferDbModel()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = transfer.Amount,
                CurrencyCode = transfer.CurrencyCode,
                FromAccountId = transfer.FromAccountId,
                ToAccountId = transfer.ToAccountId
            };

             return _context.MoneyTransfer.AddAsync(entityMoneyTransfer).AsTask();
        }
    }
}