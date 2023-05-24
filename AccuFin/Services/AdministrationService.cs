using AccuFin.Api.Models;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace AccuFin.Services
{
    public class AdministrationService
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ILocalStorageService _localStorageService;

        public AdministrationService(ISessionStorageService sessionStorageService, ILocalStorageService localStorageService)
        {
            this._sessionStorageService = sessionStorageService;
            this._localStorageService = localStorageService;
        }

        public async Task<AdministrationModel> GetCurrentAdministration()
        {
            var sessionItem = await _sessionStorageService.GetItemAsync<AdministrationModel>("current_administration");
            if (sessionItem != null)
            {
                return sessionItem;
            }

            var localItem = await _localStorageService.GetItemAsync<AdministrationModel>("current_administration");
            if (localItem != null)
            {
                await _sessionStorageService.SetItemAsync<AdministrationModel>("current_administration", localItem);
            }
            return localItem;
        }

        public async Task SetCurrentAdministration(AdministrationModel model)
        {
            await _localStorageService.SetItemAsync<AdministrationModel>("current_administration", model);
            await _sessionStorageService.SetItemAsync<AdministrationModel>("current_administration", model);
        }
    }
}
