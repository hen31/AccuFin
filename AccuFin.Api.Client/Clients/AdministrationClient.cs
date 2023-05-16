using AccuFin.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public class AdministrationClient : BaseClient, ICollectionSourceClient<AdministrationCollectionItem>
    {
        public AdministrationClient(HttpClient httpClient, IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "administration";
        }

        public Task<Response<FinCollection<AdministrationCollectionItem>, List<ValidationError>>> GetCollectionAsync(int page, int pageSize)
        {
            return DoGetRequest<FinCollection<AdministrationCollectionItem>, List<ValidationError>>($"?page={page}&pageSize={pageSize}");
        }

        public Task<Response<AdministrationModel, List<ValidationError>>> AddAdministrationAsync(AdministrationModel administration)
        {
            return DoPostRequest<AdministrationModel, List<ValidationError>>("", administration);
        }
    }
}

