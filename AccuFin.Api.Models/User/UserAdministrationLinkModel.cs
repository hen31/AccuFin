using AccuFin.Api.Models.User;
using System;
using System.Collections.Generic;

namespace AccuFin.Api.Models
{
    public class UserAdministrationLinkModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        
        public string Name { get; set; }
        public string Email { get; set; }
        public List<AdministrationRole> Roles { get; set; } = new List<AdministrationRole>();
    }

    public class AdministrationRole
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public static List<AdministrationRole> GetRoles()
        {
            /*
                      None = 0,
        Owner = 1,
        Employee = 2,
        Controller = 4,
        SalaryAdmin = 8,
        Sales = 16,
        Management = 32,
        BackOffice = 64*/
            List<AdministrationRole> roles = new List<AdministrationRole>();
            roles.Add(new AdministrationRole()
            {
                Value = 1,
                Name = "Eigenaar"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 2,
                Name = "Werknemer"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 4,
                Name = "Controller"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 8,
                Name = "Salaris"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 16,
                Name = "Sales"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 32,
                Name = "Management"
            });
            roles.Add(new AdministrationRole()
            {
                Value = 64,
                Name = "Backoffice"
            });
            return roles;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            return obj is AdministrationRole role && role.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
