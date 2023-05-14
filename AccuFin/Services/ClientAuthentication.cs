using AccuFin.Api.Client;
using AccuFin.Api.Client.Authentication;
using Blazored.LocalStorage;
using System;
using System.Threading.Tasks;

namespace AccuFin.Services
{
    public class ClientAuthentication : IClientAuthentication
    {
        private readonly ILocalStorageService _localStorage;

        public ClientAuthentication(ILocalStorageService localStorage)
        {
            this._localStorage = localStorage;
        }

        public string GetClientId() => "AccuFin.Online";

        public async Task<string> GetRefreshTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("refresh_token");
        }

        public async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsync<string>("token");
        }

        public async Task SetRefreshTokenAsync(string token)
        {
            await _localStorage.SetItemAsync("refresh_token", token);
        }

        public async Task SetToken(string token)
        {
            await _localStorage.SetItemAsync("token", token);
        }

    }
}
