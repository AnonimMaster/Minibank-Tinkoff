using Minibank.Core.Domains.CurrencyConvert;

namespace Minibank.Web.Controllers.MoneyTransfers.Dto
{
    public class MoneyTransferDto
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}