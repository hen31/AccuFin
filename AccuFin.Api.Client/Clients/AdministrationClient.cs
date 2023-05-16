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

        public Task<Response<FinCollection<AdministrationCollectionItem>, List<ValidationError>>> GetCollectionAsync(int page, int pageSize, string[] orderBy)
        {
            return DoGetRequest<FinCollection<AdministrationCollectionItem>, List<ValidationError>>(GenerateCollectionParametersUrl(page, pageSize, orderBy));
        }

        private static string GenerateCollectionParametersUrl(int page, int pageSize, string[] orderBy)
        {
            string parametersUrl = $"?page={page}&pageSize={pageSize}";
            if (orderBy == null || orderBy.Length == 0)
            {
                return parametersUrl;
            }
            return parametersUrl + $"&orderby={string.Join(',', orderBy)}";
        }

        public Task<Response<AdministrationModel, List<ValidationError>>> AddAdministrationAsync(AdministrationModel administration)
        {
            return DoPostRequest<AdministrationModel, List<ValidationError>>("", administration);
        }
    }
}

