using AccuFin.Api.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AccuFin.Api.Client
{
    public static class StartupExtensions
    {
        public static void AddAccuFinModels(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddValidatorsFromAssemblyContaining<CurrentUserModel>();
        }



        /*private static Type? FindEntityType(Type type)
        {
            var entityType = type.GenericTypeArguments.FirstOrDefault(b => typeof(IEntity).IsAssignableFrom(b));
            if (entityType == null)
            {
                if (type.BaseType == typeof(EFRepository) || type.BaseType == null)
                {
                    return null;
                }
                return FindEntityType(type.BaseType);
            }
            return entityType;
        }*/
    }
}
