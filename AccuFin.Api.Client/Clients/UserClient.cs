using AccuFin.Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public class UserClient : BaseClient
    {
        public UserClient(HttpClient httpClient, IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "user";
        }

        public Task<Response<CurrentUserModel>> GetCurrentUserAsync()
        {
            return DoGetRequest<CurrentUserModel>("");
        }

        public Task<Response<CurrentUserModel, List<ValidationError>>> UpdateCurrentUserAsync(CurrentUserModel currentUserModel)
        {
            return DoPostRequest<CurrentUserModel, List<ValidationError>>("", currentUserModel);
        }


    }
}
