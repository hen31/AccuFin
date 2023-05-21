using AccuFin.Api.Models;
using AccuFin.Api.Models.Authentication;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuFin.Api.Client.Authentication
{
    public class AuthenticationClient : BaseClient
    {
        public AuthenticationClient(HttpClient httpClient, IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "Authentication/";
        }


        public Task<Response<string, List<ValidationError>>> RegisterUserAsync(string email, string password)
        {
            return DoPostRequest<string, List<ValidationError>>("", new RegisterModel() { Email = email, Password = password }, false);
        }

        public Task<Response<string, List<ValidationError>>> ConfirmEmailAsync(string email, string code)
        {
            return DoPostRequest<string, List<ValidationError>>("ConfirmEmail", new ConfirmEmailModel() { Email = email, Code = code }, false);
        }

        public Task<Response<string, List<ValidationError>>> SendResetCodeAsync(string email, bool userInitiated)
        {
            return DoPostRequest<string, List<ValidationError>>("SendResetCode", new ResetPasswordModel() { Email = email, ChangePassword = userInitiated }, false);
        }

        public Task<Response<string, List<ValidationError>>> ResetPasswordWithCodeAsync(string email, string password, string code)
        {
            return DoPostRequest<string, List<ValidationError>>("ResetPasswordWithCode", new ResetPasswordWithCodeModel() { Email = email, ResetCode = code, Password = password }, false);
        }

        public Task<Response<string>> CheckMe()
        {
            return DoGetRequest<string>("me");
        }

        public Task<Response<TokenResult, List<ValidationError>>> LoginAsync(string email, string password)
        {
            return DoPostRequest<TokenResult, List<ValidationError>>("login", new LoginModel() { Email = email, Password = password, ClientId = ContextProvider.GetClientId() }, false);
        }

        public  Task<Response<TokenResult, List<ValidationError>>> RefreshTokenAsync(string token)
        {
            return DoPostRequest<TokenResult, List<ValidationError>>("refresh", new RefreshTokenModel() { Token = token }, false); ;
        }
    }
}