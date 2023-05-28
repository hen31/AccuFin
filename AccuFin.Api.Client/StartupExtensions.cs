using AccuFin.Api.Client.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AccuFin.Api.Client
{
    public static class StartupExtensions
    {
        public static void AddAccuFinClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<AuthenticationClient>();
            serviceCollection.AddScoped<UserClient>();
            serviceCollection.AddScoped<AdministrationClient>();
            serviceCollection.AddScoped<BankIntegrationClient>();
            
        }
        
    }
}
