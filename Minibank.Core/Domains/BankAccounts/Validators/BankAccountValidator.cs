using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;

namespace Minibank.Core.Domains.BankAccounts.Validators
{
    public class BankAccountValidator: AbstractValidator<BankAccount>
    {
        public BankAccountValidator(IBankAccountRepository bankAccountRepository)
        {
            RuleFor(x => x.CurrencyCode).IsInEnum().WithName("Код валюты").WithMessage("указан не правильно.");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithName("Счет").WithMessage("Не может быть отрицательным");
            RuleFor(x => x.Amount).NotEmpty().WithName("Счет").WithMessage("не может быть пустым.");
        }
    }
}