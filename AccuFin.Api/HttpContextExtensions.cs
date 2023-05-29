using AccuFin.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccuFin.Api
{
    public static class HttpContextExtensions
    {
        public static string GetCurrentUserEmail(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(b => b.Type == ClaimTypes.Email)?.Value;
        }

    
    }
}
