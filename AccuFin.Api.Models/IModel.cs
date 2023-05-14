using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace AccuFin.Api.Models
{
    public interface IModel<T>
    {
        public IValidator<T> GetValidator();
   
    }
}