using AccuFin.Api.Client;
using AccuFin.Api.Models.BankIntegration;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AccuFin.Pages
{
    public partial class CompleteIntegration : ComponentBase
    {
        [Parameter]
        public Guid AdministrationId { get; set; }

        [Inject]
        public BankIntegrationClient BankIntegrationClient { get; set; }
        public Response<ICollection<ExternalBankAccountModel>> BankAccounts { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            BankAccounts = await BankIntegrationClient.GetAccountInformationAsync(AdministrationId);
        }

    }
}
