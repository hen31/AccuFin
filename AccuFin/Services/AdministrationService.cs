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

        public async Task<AdministrationCollectionItem> GetCurrentAdministration()
        {
            var sessionItem = await _sessionStorageService.GetItemAsync<AdministrationCollectionItem>("current_administration");
            if (sessionItem != null)
            {
                return sessionItem;
            }

            var localItem = await _localStorageService.GetItemAsync<AdministrationCollectionItem>("current_administration");
            if (localItem != null)
            {
                await _sessionStorageService.SetItemAsync<AdministrationCollectionItem>("current_administration", localItem);
            }
            return localItem;
        }

        public async Task SetCurrentAdministration(AdministrationCollectionItem model)
        {
            await _localStorageService.SetItemAsync<AdministrationCollectionItem>("current_administration", model);
            await _sessionStorageService.SetItemAsync<AdministrationCollectionItem>("current_administration", model);
        }
    }
}
