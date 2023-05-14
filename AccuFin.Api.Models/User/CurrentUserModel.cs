using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models
{
    public class CurrentUserModel : IModel<CurrentUserModel>
    {
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string MobileNumber { get; set; }

        public IValidator<CurrentUserModel> GetValidator()
        {
            return new CurrentUserModelValidator();
        }
    }

    public class CurrentUserModelValidator : AbstractValidator<CurrentUserModel>
    {
        public CurrentUserModelValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Telephone).NotEmpty();
        }
    }
}
