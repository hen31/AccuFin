using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuFin.Api.Models
{
    public static class ValidatorExtensions
    {
        public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<T>(this IModel<T> model)
        {
            return async (model, propertyName) =>
            {
                var result = await ((IModel<T>)model).GetValidator().ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}
