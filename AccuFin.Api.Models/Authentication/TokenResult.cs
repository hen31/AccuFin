using System;
using System.Collections.Generic;
using System.Text;

namespace AccuFin.Api.Models.Authentication
{
    public class TokenResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
