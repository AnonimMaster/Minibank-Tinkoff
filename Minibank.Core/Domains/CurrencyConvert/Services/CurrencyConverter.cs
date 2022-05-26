using FluentValidation;
using Minibank.Core.Domains.CurrencyConvert.Repositories;
using System;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.CurrencyConvert.Services
{
    public class CurrencyConverter : ICurrencyConverter
	{
		private readonly ICurrencyRateService _currencyRateService;

		public CurrencyConverter(ICurrencyRateService currencyRateService)
		{
			_currencyRateService = currencyRateService;
		}

        public async Task<double> Convert(double amount, CurrencyCode fromCurrency, CurrencyCode toCurrency)
        {
            if (amount < 0)
            {
                throw new ValidationException("Введенно отрицательное число");
            }

            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            if (fromCurrency == CurrencyCode.RUB)
            {
                return Math.Round(amount / await _currencyRateService.GetCurrencyRate(toCurrency), 2);
            }

            if (toCurrency == CurrencyCode.RUB)
            {
                return Math.Round(amount * await _currencyRateService.GetCurrencyRate(fromCurrency), 2);
            }

            var toAmountRub = await _currencyRateService.GetCurrencyRate(fromCurrency);
            var fromAmountRub = amount / await _currencyRateService.GetCurrencyRate(toCurrency);

            return Math.Round(toAmountRub * fromAmountRub, 2);
        }
	}
}
