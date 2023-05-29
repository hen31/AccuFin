using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.BankIntegration
{
    public class LinkBankAccountModel
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public string IBAN { get; set; }
        public bool Sync { get; set; }
    }
}
