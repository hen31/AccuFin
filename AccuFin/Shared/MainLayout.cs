using AccuFin.Api.Client;
using AccuFin.Api.Models;
using AccuFin.Pages;
using AccuFin.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Colors;

namespace AccuFin.Shared
{
    public partial class MainLayout
    {

        public MudAutocomplete<AdministrationCollectionItem> AdminstrationAutocomplete { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public AdministrationService AdministrationService { get; set; }

        public AdministrationModel CurrentAdministration { get; set; }

        public AdministrationCollectionItem SelectedAdministration { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAdministrations();
            CurrentAdministration = await AdministrationService.GetCurrentAdministration();
        }

        public async Task SelectedAdministrationChanged()
        {
            if (SelectedAdministration != null)
            {
                var administration = await AdministrationClient.GetAdministrationByIdAsync(SelectedAdministration.Id);
                if (!administration.Success)
                {
                    Snackbar.Add("Ophalen administratie is mislukt", Severity.Error);
                    return;
                }
                CurrentAdministration = administration.Data;
                await AdministrationService.SetCurrentAdministration(CurrentAdministration);

            }
            await AdminstrationAutocomplete.Clear();
        }
        private async Task GetAdministrations()
        {
            Administrations = await AdministrationClient.GetMyAdministrations();
            if (!Administrations.Success)
            {
                Snackbar.Add("Ophalen administraties is mislukt", Severity.Error);
            }
        }

        private async Task<IEnumerable<AdministrationCollectionItem>> SearchAdministration(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Administrations.Data;
            }
            return Administrations.Data.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.AdministrationRegistryCode.StartsWith(value, StringComparison.InvariantCultureIgnoreCase));
        }

        MudTheme DefaultTheme = new MudTheme()
        {
            Palette = new PaletteLight()
            {
                Primary = new MudBlazor.Utilities.MudColor("#4D65F1"),
                AppbarBackground = new MudBlazor.Utilities.MudColor("#4D65F1"),

            },
            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "0px"
            }

        };
        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        [Inject]
        public AdministrationClient AdministrationClient { get; set; }

        [Inject]
        private ClientAuthentication ClientAuthentication { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public Response<IEnumerable<AdministrationCollectionItem>> Administrations { get; private set; }

        public async Task LogOff()
        {
            await ClientAuthentication.SetToken(null);
            await ClientAuthentication.SetRefreshTokenAsync(null);
            NavigationManager.NavigateTo("/login");
        }

    }
}
