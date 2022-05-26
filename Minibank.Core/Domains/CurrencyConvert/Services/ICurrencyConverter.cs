using System.Threading.Tasks;

namespace Minibank.Core.Domains.CurrencyConvert.Services
{
    public interface ICurrencyConverter
    {
        public Task<double> Convert(double amount, CurrencyCode fromCurrency, CurrencyCode toCurrency);
    }
}
