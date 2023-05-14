using AccuFin.Api.Client.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AccuFin.Api.Client
{
    public static class StartupExtensions
    {
        public static void AddAccuFinClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<AuthenticationClient>();
            serviceCollection.AddScoped<UserClient>();
        }
        
    }
}
