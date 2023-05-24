using Microsoft.AspNetCore.Mvc;

namespace AccuFin.Api.Controllers
{
    public static class ControllerExtensions
    {

        public static Guid GetFinUserId(this ControllerBase controllerBase)
        {
            return Guid.TryParse(controllerBase.HttpContext.User.Claims.FirstOrDefault(b => b.Type == "fui")?.Value, out Guid userId) ? userId : Guid.Empty;

        }
    }
}
