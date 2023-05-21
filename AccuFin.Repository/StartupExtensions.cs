using Microsoft.Extensions.DependencyInjection;

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
