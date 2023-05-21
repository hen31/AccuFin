using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
