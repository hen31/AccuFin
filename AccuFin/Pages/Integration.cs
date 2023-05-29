using AccuFin.Api.Client;
using AccuFin.Api.Models;
using AccuFin.Api.Models.BankIntegration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AccuFin.Pages
{
    public partial class Integration : ComponentBase
    {
        [Parameter]
        public Guid AdministrationId { get; set; }
        [Inject]
        public BankIntegrationClient BankIntegrationClient { get; set; }
        [Inject]
        public AdministrationClient AdministrationClient { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public Response<ICollection<BankModel>> Banks { get; private set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }

        public BankModel SelectedBank { get; set; }
        public Response<BankLinkAuthorizationModel> BankLink { get; set; }
        public AdministrationCollectionItem SelectedAdministration { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Banks = await BankIntegrationClient.GetCollectionAsync();
            if (!Banks.Success)
            {
                Snackbar.Add("Ophalen banken is mislukt", Severity.Error);
            }
            var administrations = await AdministrationClient.GetMyAdministrations();
            if (!administrations.Success)
            {
                Snackbar.Add("Ophalen administraties is mislukt", Severity.Error);
            }
            else
            {
                SelectedAdministration = administrations.Data.FirstOrDefault(b=> b.Id ==  AdministrationId);
            }
        }

        public async Task GetLink()
        {
            if (SelectedBank != null && SelectedAdministration != null)
            {
                BankLink = await BankIntegrationClient.GetLinkAsync(SelectedBank, SelectedAdministration.Id);
                NavigationManager.NavigateTo(BankLink.Data.Link);
            }
        }
    }
}
