using FluentValidation;

namespace AccuFin.Api.Models
{
    public interface IModel<T>
    {
        public IValidator<T> GetValidator();
   
    }
}