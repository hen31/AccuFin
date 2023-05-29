﻿using AccuFin.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AccuFin.Api.Client
{
    public class AdministrationClient : BaseClient, ICollectionSourceClient<AdministrationCollectionItem>
    {
        public AdministrationClient(HttpClient httpClient, IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "administration";
        }

        public Task<Response<FinCollection<AdministrationCollectionItem>, List<ValidationError>>> GetCollectionAsync(int page, int pageSize, string[] orderBy, string singleSearchText)
        {
            return DoGetRequest<FinCollection<AdministrationCollectionItem>, List<ValidationError>>(GenerateCollectionParametersUrl(page, pageSize, orderBy, singleSearchText));
        }
        public Task<Response<AdministrationModel>> GetAdministrationByIdAsync(Guid id)
        {
            return DoGetRequest<AdministrationModel>($"/{id}");
        }

        public Task<Response<IEnumerable<AdministrationCollectionItem>>> GetMyAdministrations()
        {
            return DoGetRequest<IEnumerable<AdministrationCollectionItem>>($"/myadministrations");
        }
        public Task<Response<AdministrationModel, List<ValidationError>>> UpdateAdministrationAsync(AdministrationModel administration)
        {
            return DoPostRequest<AdministrationModel, List<ValidationError>>($"/{administration.Id}", administration);
        }
     

        public Task<Response<AdministrationModel, List<ValidationError>>> AddAdministrationAsync(AdministrationModel administration)
        {
            return DoPostRequest<AdministrationModel, List<ValidationError>>("", administration);
        }

        public Task<Response<bool>> DeleteAdministrationAsync(Guid id)
        {
            return DoPostRequest<bool>($"/delete/{id}");
        }
    }
}

