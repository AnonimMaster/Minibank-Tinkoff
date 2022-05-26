using Minibank.Core.Domains.CurrencyConvert;

namespace Minibank.Core.Domains.MoneyTransfers
{
    public class MoneyTransfer
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}