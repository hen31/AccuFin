using Microsoft.AspNetCore.Components;
using System.Web;

namespace AccuFin.Services
{
    public class PreviousNavigationService
    {
        private NavigationManager _navigationManager;

        public PreviousNavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public string GetReturnUrl()
        {
            if (_navigationManager.TryGetQueryString<string>("returnurl", out string parametersForReturn))
            {
                return parametersForReturn;
            }
            return null;
        }

        public string GetUrlQuery()
        {
            return "?returnurl=" + HttpUtility.UrlEncode(new Uri(_navigationManager.Uri).Query);
        }
    }
}
