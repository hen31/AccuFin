using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models
{
    public class TransactionCollectionItem
    {
        public ulong Id { get; set; }
        public string IBAN { get; set; }
        public string ToIBAN { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
