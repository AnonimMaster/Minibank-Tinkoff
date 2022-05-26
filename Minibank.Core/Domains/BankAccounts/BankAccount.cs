using System;
using Minibank.Core.Domains.CurrencyConvert;

namespace Minibank.Core.Domains.BankAccounts
{
    public class BankAccount
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public bool IsLocked { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}