using FluentValidation;
using Minibank.Core.Domains.CurrencyConvert;
using Minibank.Core.Domains.CurrencyConvert.Repositories;
using Minibank.Data.HttpClients.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minibank.Data.HttpClients
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<double> GetCurrencyRate(CurrencyCode currencyCode)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://www.cbr-xml-daily.ru/daily_json.js")
            };

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ValidationException("Ошибка запроса");
            }

            CurrencyResponse modelCurrencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            return (double)modelCurrencyResponse.Valute[$"{currencyCode}"].Value;
        }
    }
}