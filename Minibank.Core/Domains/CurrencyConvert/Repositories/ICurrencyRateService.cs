using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.CurrencyConvert.Repositories
{
    public interface ICurrencyRateService
    {
        public Task<double> GetCurrencyRate(CurrencyCode currencyCode);
    }
}
