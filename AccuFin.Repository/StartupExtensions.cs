using AccuFin.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AccuFin.Repository
{
    public static class StartupExtensions
    {
        public static void AddAccuFinRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<UserRepository>();
            serviceCollection.AddScoped<AdministrationRepository>();
        }
        
    }
}
