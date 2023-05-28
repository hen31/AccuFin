using AccuFin.Api.Client;
using AccuFin.Api.Models;
using AccuFin.Api.Models.BankIntegration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AccuFin.Pages
{
    public partial class Integration : ComponentBase
    {
        [Inject]
        public BankIntegrationClient BankIntegrationClient { get; set; }
        [Inject]
        public AdministrationClient AdministrationClient { get; set; }
        public Response<ICollection<BankModel>> Banks { get; private set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }

        public BankModel SelectedBank { get; set; }
        public Response<BankLinkAuthorizationModel> BankLink { get; set; }
        public AdministrationCollectionItem SelectedAdministration { get; set; }
        public Response<IEnumerable<AdministrationCollectionItem>> Administrations { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Banks = await BankIntegrationClient.GetCollectionAsync();
            if (!Banks.Success)
            {
                Snackbar.Add("Ophalen banken is mislukt", Severity.Error);
            }
            Administrations = await AdministrationClient.GetMyAdministrations();
            if (!Administrations.Success)
            {
                Snackbar.Add("Ophalen administraties is mislukt", Severity.Error);
            }
        }

        public async Task GetLink()
        {
            if (SelectedBank != null && SelectedAdministration != null)
            {
                BankLink = await BankIntegrationClient.GetLinkAsync(SelectedBank, SelectedAdministration.Id);
            }
        }
    }
}
