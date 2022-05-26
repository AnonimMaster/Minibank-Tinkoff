using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.MoneyTransfers.Services;
using Minibank.Web.Controllers.BankAccounts.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Web.Controllers.MoneyTransfers.Dto;

namespace Minibank.Web.Controllers.MoneyTransfers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyTransfersController : ControllerBase
    {
        private readonly IMoneyTransferService _moneyTransferService;

        public MoneyTransfersController(IMoneyTransferService moneyTransferService)
        {
            _moneyTransferService = moneyTransferService;
        }

        [HttpGet]
        public async Task<List<MoneyTransferDto>> GetAllTransfers()
        {
            var transfers = await _moneyTransferService.GetAllTransfers();

            return transfers.Select(i => new MoneyTransferDto()
            {
                Id = i.Id,
                Amount = i.Amount,
                CurrencyCode = i.CurrencyCode,
                FromAccountId = i.FromAccountId,
                ToAccountId = i.ToAccountId
            }).ToList();
        }

        [HttpGet("{bankAccountId}")]
        public async Task<List<MoneyTransferDto>> GetAllTransfersBankAccount(string bankAccountId)
        {
            var transfers = await _moneyTransferService.GetAllTransfersBankAccount(bankAccountId);

            return transfers.Select((i => new MoneyTransferDto()
            {
                Id = i.Id,
                Amount = i.Amount,
                CurrencyCode = i.CurrencyCode,
                FromAccountId = i.FromAccountId,
                ToAccountId = i.ToAccountId
            })).ToList();
        }

        [HttpGet("CalculateCommission")]
        public async Task<double> CalculateCommission(double amount, string fromBankAccountId, string toBankAccountId)
        {
            return await _moneyTransferService.CalculateCommission(amount, fromBankAccountId, toBankAccountId);
        }

        [HttpPost]
        public Task TransferMoney(double amount, string fromBankAccountId, string toBankAccountId)
        {
            return _moneyTransferService.TransferMoney(amount, fromBankAccountId, toBankAccountId);
        }
    }
}
