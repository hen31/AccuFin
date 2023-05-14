using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Data.Entities
{
    public class AuthorizedUser : BaseEntityGuidId
    {
        public string EmailAdress { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string MobileNumber { get; set; }
    }
}
