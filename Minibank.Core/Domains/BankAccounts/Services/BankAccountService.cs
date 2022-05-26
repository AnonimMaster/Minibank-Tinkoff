using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.CurrencyConvert;
using Minibank.Core.Domains.CurrencyConvert.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.BankAccounts.Validators;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService: IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<BankAccount> _bankAccountValidator;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUserRepository userRepository, ICurrencyRateService currencyRateService, IUnitOfWork unitOfWork, IValidator<BankAccount> bankAccountValidator)
        {
            _bankAccountRepository = bankAccountRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _bankAccountValidator = bankAccountValidator;
        }
        public async Task<BankAccount> GetBankAccount(string id)
        {
            return await _bankAccountRepository.GetBankAccount(id);
        }

        public async Task CreateBankAccount(string userId, CurrencyCode currencyCode, int initialAmount)
        {
            if (!await _userRepository.IsUserExists(userId))
            {
                throw new ObjectNotFoundException(
                    "Пользователя для которого вы пытаетесь создать банковский аккаунт, не существует.");
            }

            var bankAccount = new BankAccount()
            {
                Id = Guid.NewGuid().ToString(),
                CurrencyCode = currencyCode,
                UserId = userId,
                Amount = initialAmount,
                IsLocked = false,
                OpeningDate = DateTime.UtcNow
            };
            await _bankAccountValidator.ValidateAndThrowAsync(bankAccount);

            await _bankAccountRepository.CreateBankAccount(bankAccount);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<BankAccount>> GetAllBankAccounts()
        {
            return await _bankAccountRepository.GetAllBankAccounts();
        }

        public async Task LockBankAccount(string bankAccountId)
        {
            var bankAccount = await _bankAccountRepository.GetBankAccount(bankAccountId);

            if (bankAccount == null)
            {
                throw new ObjectNotFoundException("Данного банковского счета не существует.");
            }

            if (bankAccount.IsLocked)
            {
                throw new ValidationException("Данный банковский счет уже закрыт!");
            }

            if (bankAccount.Amount > 0)
            {
                throw new ValidationException(
                    "Невозможно закрыть банковский счёт, так как на нём имеются средства.");
            }

            await _bankAccountRepository.LockBankAccount(bankAccountId);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}