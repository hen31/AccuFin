﻿using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace AccuFin.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClientAuthentication _clientAuthentication;
        public JwtAuthenticationStateProvider(ClientAuthentication clientAuthentication)
        {
            _clientAuthentication = clientAuthentication;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtToken = await _clientAuthentication.GetTokenAsync();
            if (string.IsNullOrEmpty(jwtToken))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity()));
            }
            return new AuthenticationState(
                    new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(jwtToken), "jwtAuth")));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public void NotifyAuthState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
