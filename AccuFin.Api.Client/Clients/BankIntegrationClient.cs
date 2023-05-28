using AccuFin.Api.Models;
using AccuFin.Api.Models.BankIntegration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AccuFin.Api.Client
{
    public class BankIntegrationClient : BaseClient
    {
        public BankIntegrationClient(HttpClient httpClient, IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "bankintegration";
        }

        public Task<Response<ICollection<BankModel>>> GetCollectionAsync()
        {
            return DoGetRequest<ICollection<BankModel>>("/banks");
        }

        public Task<Response<BankLinkAuthorizationModel>> GetLinkAsync(BankModel bank, Guid administrationId)
        {
            return DoGetRequest<BankLinkAuthorizationModel>($"/getlink/{bank.Id}/{administrationId}");
        }

        public Task<Response<ICollection<ExternalBankAccountModel>>> GetAccountInformationAsync(Guid administrationId)
        {
            return DoGetRequest<ICollection<ExternalBankAccountModel>>($"/getaccountinformation/{administrationId}");
        }
        
    }
}

