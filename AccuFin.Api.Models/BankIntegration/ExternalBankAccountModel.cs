using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.BankIntegration
{
    public class ExternalBankAccountModel
    {
        public string ExternalAccountId { get; set; }
        public string AccountId { get; set; }
        public string IBAN { get; set; }
        public string Name { get; set; }
    }
}
