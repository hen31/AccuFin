using AccuFin.Api.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace AccuFin.Api.Models
{
    public class AdministrationModel : IModel<AdministrationModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AdministrationRegistryCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAdress { get; set; }

        public List<UserAdministrationLinkModel> Users { get; set; } = new List<UserAdministrationLinkModel>();

        public IValidator<AdministrationModel> GetValidator()
        {
            return new AdministrationModelValidator();
        }
    }
    public class AdministrationModelValidator : AbstractValidator<AdministrationModel>
    {
        public AdministrationModelValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithName("Naam");
            RuleFor(b => b.TelephoneNumber).NotEmpty().WithName("Telefoonnummer");
            RuleFor(b => b.EmailAdress).NotEmpty().EmailAddress().WithName("Emailadres");
        }
    }
}
