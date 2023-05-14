using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Data.Entities
{
    public class UserAdministrationLink : BaseEntityGuidId
    {
        public Guid UserId { get; set; }
        public AuthorizedUser User { get; set; }

        public Guid AdministrationId { get; set; }
        public Administration Administration { get; set; }
    }
}
