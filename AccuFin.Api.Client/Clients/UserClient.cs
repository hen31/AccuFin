using AccuFin.Api.Models;
using AccuFin.Api.Models.User;
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

        public Task<Response<UserModel>> GetUserByEmailadressAsync(string emailAdress)
        {
            return DoGetRequest<UserModel>($"/findbyemail/{emailAdress}");
        }


    }
}
