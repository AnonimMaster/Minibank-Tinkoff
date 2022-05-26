using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.CurrencyConvert.Services;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Exceptions;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public class MoneyTransferService: IMoneyTransferService
    {
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IUnitOfWork _unitOfWork;

        public MoneyTransferService(IMoneyTransferRepository moneyTransferRepository, IBankAccountRepository bankAccountRepository,
            ICurrencyConverter currencyConverter, IUnitOfWork unitOfWork)
        {
            _moneyTransferRepository = moneyTransferRepository;
            _bankAccountRepository = bankAccountRepository;
            _currencyConverter = currencyConverter;
            _unitOfWork = unitOfWork;
        }

        public Task<List<MoneyTransfer>> GetAllTransfers()
        {
            return _moneyTransferRepository.GetAllTransfers();
        }

        public async Task<List<MoneyTransfer>> GetAllTransfersBankAccount(string bankAccountId)
        {
            var transfers = await _moneyTransferRepository.GetAllTransfers();
            return transfers.Where(i => i.FromAccountId == bankAccountId || i.ToAccountId == bankAccountId).ToList();
        }

        public async Task<double> CalculateCommission(double amount, string fromBankAccountId, string toBankAccountId)
        {
            var fromBankAccount = await _bankAccountRepository.GetBankAccount(fromBankAccountId);
            var toBankAccount = await _bankAccountRepository.GetBankAccount(toBankAccountId);

            if (fromBankAccount == null || toBankAccount == null)
            {
                throw new ObjectNotFoundException(
                    "Банковский счёт отправителя или получателя не существует, проверьте правильно ли введены данные.");
            }

            if (fromBankAccount.UserId == toBankAccount.UserId)
            {
                return 0;
            }

            return Math.Round(amount * 0.02f, 2);
        }

        public async Task TransferMoney(double amount, string fromBankAccountId, string toBankAccountId)
        {
            if (amount <= 0)
            {
                throw new ValidationException("Нельзя перевести такую сумму");
            }

            if (fromBankAccountId == toBankAccountId)
            {
                throw new ValidationException("Адресс отправителя и получателя совпадают!");
            }

            var fromBankAccount = await _bankAccountRepository.GetBankAccount(fromBankAccountId);
            var toBankAccount = await _bankAccountRepository.GetBankAccount(toBankAccountId);

            if (fromBankAccount == null || toBankAccount == null)
            {
                throw new ObjectNotFoundException(
                    "Банковский счёт отправителя или получателя не существует, проверьте правильно ли введены данные.");
            }

            if (fromBankAccount.IsLocked || toBankAccount.IsLocked)
            {
                throw new ValidationException(
                    "Банковский аккаунт с которым вы пытаетесь произвести операцию заблокирован");
            }

            if (fromBankAccount.Amount < amount)
            {
                throw new ValidationException(
                    "На счету банковского аккаунт недостаточно средств для перевода");
            }

            fromBankAccount.Amount -= amount;
            await _bankAccountRepository.UpdateBankAccount(fromBankAccount);

            var commission = await CalculateCommission(amount, fromBankAccountId, toBankAccountId);

            amount -= commission;
            amount = await _currencyConverter.Convert(amount, fromBankAccount.CurrencyCode, toBankAccount.CurrencyCode);
            var transferCurrencyCode = toBankAccount.CurrencyCode;

            toBankAccount.Amount += amount;
            await _bankAccountRepository.UpdateBankAccount(toBankAccount);

            await _moneyTransferRepository.Create(new MoneyTransfer()
            {
                Amount = amount,
                CurrencyCode = transferCurrencyCode,
                FromAccountId = fromBankAccountId,
                ToAccountId = toBankAccountId
            });
            await _unitOfWork.SaveChangesAsync();
        }
    }
}