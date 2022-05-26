using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.CurrencyConvert;
using Minibank.Core.Exceptions;
using Minibank.Web.Controllers.BankAccounts.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.BankAccounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost]
        public Task CreateBankAccount(string userId, CurrencyCode currencyCode, int initialAmount)
        {
            return _bankAccountService.CreateBankAccount(userId, currencyCode, initialAmount);
        }

        [HttpPost("{bankAccountId}/lock")]
        public Task LockBankAccount(string bankAccountId)
        {
            return _bankAccountService.LockBankAccount(bankAccountId);
        }

        [HttpGet("{bankAccountId}")]
        public async Task<BankAccountDto> GetBankAccount(string bankAccountId)
        {
            var bankAccount = await _bankAccountService.GetBankAccount(bankAccountId);

            if (bankAccount == null)
            {
                throw new ObjectNotFoundException($"Банковского аккаунта с Id = {bankAccountId} не существует.");
            }

            return new BankAccountDto()
            {
                Id = bankAccount.Id,
                UserId = bankAccount.UserId,
                CurrencyCode = bankAccount.CurrencyCode,
                IsLocked = bankAccount.IsLocked,
                Amount = bankAccount.Amount,
                OpeningDate = bankAccount.OpeningDate,
                ClosingDate = bankAccount.ClosingDate
            };
        }

        [HttpGet]
        public async Task<List<BankAccountDto>> GetAllBankAccounts()
        {
            var bankAccounts = await _bankAccountService.GetAllBankAccounts();

            return bankAccounts.Select(i => new BankAccountDto()
            {
                Id = i.Id,
                UserId = i.UserId,
                CurrencyCode = i.CurrencyCode,
                IsLocked = i.IsLocked,
                Amount = i.Amount,
                OpeningDate = i.OpeningDate,
                ClosingDate = i.ClosingDate
            }).ToList();
        }
    }
}
