using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public interface IClientAuthentication
    {
        public Task SetRefreshTokenAsync(string token);
        public Task<string> GetRefreshTokenAsync();
        public Task<string> GetToken();
        public Task SetToken(string token);

        public string GetClientId();
    }
}
