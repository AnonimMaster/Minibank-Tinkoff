using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.CurrencyConvert;
using Minibank.Core.Domains.CurrencyConvert.Services;

namespace Minibank.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverter _currencyConverter;

        public CurrencyConverterController(ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
        }

        [HttpGet("Convert")]
        public async Task<double> Convert(double amount, CurrencyCode fromCurrencyCode, CurrencyCode toCurrencyCode)
        {
            return await _currencyConverter.Convert(amount, fromCurrencyCode, toCurrencyCode);
        }
    }
}
