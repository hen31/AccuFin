using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Data.Entities
{
    public class Transaction : BaseEntityULongId
    {
        [ForeignKey(nameof(Administration))]
        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }

        public string AccountId { get; set; }
        public string IBAN { get; set; }
        public string FromIBAN { get; set; }
        public string ExternalId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UnstructeredInformation { get; set; }
        public string Debtor { get; set; }
        public string ToIBAN { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
