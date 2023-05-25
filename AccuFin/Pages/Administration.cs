using AccuFin.Api.Client;
using AccuFin.Api.Models;
using AccuFin.Shared.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AccuFin.Pages
{
    public partial class Administration : ComponentBase
    {
        [Inject]
        public AdministrationClient AdministrationClient { get; set; }

        [Inject]
        public ISnackbar Snackbar { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }
        [Inject]
        public UserClient UserClient { get; set; }
        [Parameter]
        public Guid Id { get; set; }

        public string AddEmailAdress { get; set; }

        public List<AdministrationRole> Roles { get; set; }

        public MudForm Form { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Roles = AdministrationRole.GetRoles();
        }

        public async Task FilesChanged(InputFileChangeEventArgs e, AdministrationModel model)
        {
            var buffers = new byte[e.File.Size];
            await e.File.OpenReadStream().ReadAsync(buffers);
            model.ImageData = Convert.ToBase64String(buffers);
            model.ImageFileName = e.File.Name;
        }

        public async Task<InitializeEditFormResult<AdministrationModel>> GetModelAsync()
        {
            if (Id == Guid.Empty)
            {
                return new InitializeEditFormResult<AdministrationModel>(new AdministrationModel(), null);
            }
            else
            {
                var response = await AdministrationClient.GetAdministrationByIdAsync(Id);
                if (response.Success)
                {
                    return new InitializeEditFormResult<AdministrationModel>(response.Data, response);
                }
                else
                {
                    return new InitializeEditFormResult<AdministrationModel>(null, response);
                }
            }
        }

        private async Task<Response<bool>> DeleteModelAsync(AdministrationModel model)
        {
            return await AdministrationClient.DeleteAdministrationAsync(model.Id);
        }

        private async Task<Response<AdministrationModel, List<ValidationError>>> OnSave(AdministrationModel model)
        {
            if (Id == Guid.Empty)
            {
                return await AdministrationClient.AddAdministrationAsync(model);
            }
            else
            {
                return await AdministrationClient.UpdateAdministrationAsync(model);
            }
        }

        public async Task AddUserByEmail(AdministrationModel model)
        {
            if (!string.IsNullOrWhiteSpace(AddEmailAdress))
            {
                var response = await UserClient.GetUserByEmailadressAsync(AddEmailAdress);
                if (!response.Success)
                {
                    Snackbar.Add("Fout bij ophalen gegevens", Severity.Error);
                }

                if (response.Data == null)
                {
                    var parameters = new DialogParameters();
                    parameters.Add("ContentText", "Geen gebruiker met emailadres gevonden. Wilt u deze toevoegen?");
                    parameters.Add("CancelText", "Sluiten");
                    parameters.Add("ConfirmButton", false);

                    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
                    var dialog = await DialogService.ShowAsync<FinDialog>("Niet gevonden", parameters, options);
                }
                else
                {
                    model.Users.Add(new UserAdministrationLinkModel() { Email = response.Data.Email, Name = response.Data.Name, UserId = response.Data.Id });
                }

            }
        }
    }
}
