using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Data.Entities
{
    public class BankIntegration : BaseEntityGuidId
    {
        [ForeignKey(nameof(AuthorizedUser))]
        public Guid UserId { get; set; }
        public AuthorizedUser AuthorizedUser { get; set; }
        [ForeignKey(nameof(Administration))]
        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }
        public string Institution { get; set; }
        
        public DateTime InitializedOn { get; set; }
        public string Link { get; set; }
        public string Reference { get; set; }
        public string ClientId { get; set; }
        public string ExternalLinkId { get; set; }
        public bool Accepted { get; set; }
        public DateTime AcceptedOn { get; set; }
    }
}
