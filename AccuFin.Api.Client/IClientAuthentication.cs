using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public interface IClientAuthentication
    {
        public Task SetRefreshTokenAsync(string token);
        public Task<string> GetRefreshTokenAsync();
        public Task<string> GetTokenAsync();
        public Task SetToken(string token);

        public string GetClientId();
    }
}
