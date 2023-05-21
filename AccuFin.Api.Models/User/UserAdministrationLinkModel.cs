using System;

namespace AccuFin.Api.Models.User
{
    public class UserAdministrationLinkModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRoleInAdministration Role { get; set; }
    }
}
