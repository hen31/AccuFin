using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Data.Entities
{
    public class LinkBankAccount : BaseEntityGuidId
    {
        [ForeignKey(nameof(Administration))]
        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }

        public string AccountId { get; set; }
        public string IBAN { get; set; }
        public bool Sync { get; set; }
        public DateTime LastSync { get; set; }
    }
}
