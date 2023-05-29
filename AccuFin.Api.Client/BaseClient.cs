using AccuFin.Api.Client.Authentication;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccuFin.Api.Client
{
    public class BaseClient
    {
        protected string BaseUrl = "https://localhost:7058/";

        protected string Area { get; set; }

        protected HttpClient HttpClient { get; }
        protected IClientAuthentication ContextProvider { get; }

        public BaseClient(HttpClient httpClient, IClientAuthentication contextProvider)
        {
            HttpClient = httpClient;
            ContextProvider = contextProvider;
        }
        private static Task<Response<ResultType, ErrorDataType>> HandleResponse<ResultType, ErrorDataType>(HttpResponseMessage httpReponse)
        {
            return HandleResponse<ResultType, ErrorDataType, Response<ResultType, ErrorDataType>>(httpReponse);
        }

        private static async Task<ResponseType> HandleResponse<ResultType, ErrorDataType, ResponseType>(HttpResponseMessage httpReponse) where ResponseType : Response<ResultType, ErrorDataType>, new()
        {
            if (!httpReponse.IsSuccessStatusCode)
            {
                var errorAsString = await httpReponse.Content.ReadAsStringAsync();
                if (typeof(ErrorDataType) == typeof(string) )
                {
                    return new ResponseType() { Success = false, ErrorData = (ErrorDataType)(object)errorAsString, StatusCode = httpReponse.StatusCode };
                }
                if(httpReponse.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    return new ResponseType() { Success = false, ErrorMessage = errorAsString, StatusCode = httpReponse.StatusCode };
                }
                return new ResponseType() { Success = false, ErrorData = JsonConvert.DeserializeObject<ErrorDataType>(errorAsString), StatusCode = httpReponse.StatusCode };
            }
            var reponseAsString = await httpReponse.Content.ReadAsStringAsync();
            if (typeof(ResultType) == typeof(string))
            {
                return new ResponseType() { Success = true, Data = (ResultType)(object)reponseAsString };
            }
            var result = JsonConvert.DeserializeObject<ResultType>(reponseAsString);
            return new ResponseType() { Success = true, Data = result };
        }

        protected Task<Response<ResultType>> DoPostRequest<ResultType>(string url, object postBody = null)
        {
            return InternalDoPostRequest<ResultType, string, Response<ResultType>>(url, postBody);
        }
        protected Task<Response<ResultType, ErrorDataType>> DoPostRequest<ResultType, ErrorDataType>(string url, object postBody = null, bool refreshKeyIfNeeded = true)
        {
            return InternalDoPostRequest<ResultType, ErrorDataType, Response<ResultType, ErrorDataType>>(url, postBody, refreshKeyIfNeeded);
        }
        private async Task<ResponseType> InternalDoPostRequest<ResultType, ErrorDataType, ResponseType>(string url, object postBody = null, bool refreshKeyIfNeeded = true) where ResponseType : Response<ResultType, ErrorDataType>, new()
        {
            await AddAuthenticationHeader();
            HttpResponseMessage httpReponse;
            if (postBody == null)
            {
                httpReponse = await HttpClient.PostAsync(BaseUrl + Area + url, null);
                if (refreshKeyIfNeeded)
                {
                    httpReponse = await TryRefreshingTokenIfNeededAsync(() => HttpClient.PostAsync(BaseUrl + Area + url, null), httpReponse);
                }
            }
            else
            {
                var jsonContent = JsonConvert.SerializeObject(postBody);
                httpReponse = await HttpClient.PostAsync(BaseUrl + Area + url, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
                if (refreshKeyIfNeeded)
                {
                    httpReponse = await TryRefreshingTokenIfNeededAsync(() => HttpClient.PostAsync(BaseUrl + Area + url, new StringContent(jsonContent, Encoding.UTF8, "application/json")), httpReponse);
                }
            }
            return await HandleResponse<ResultType, ErrorDataType, ResponseType>(httpReponse);
        }

        private async Task AddAuthenticationHeader()
        {
            string token = await ContextProvider.GetTokenAsync();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        protected async Task<Response<ResultType, ErrorDataType>> DoGetRequest<ResultType, ErrorDataType>(string url)
        {
            await AddAuthenticationHeader();
            var httpReponse = await HttpClient.GetAsync(BaseUrl + Area + url);
            httpReponse = await TryRefreshingTokenIfNeededAsync(() => HttpClient.GetAsync(BaseUrl + Area + url), httpReponse);
            return await HandleResponse<ResultType, ErrorDataType>(httpReponse);

        }

        protected async Task<Response<ResultType>> DoGetRequest<ResultType>(string url)
        {
            await AddAuthenticationHeader();
            HttpResponseMessage httpReponse = await HttpClient.GetAsync(BaseUrl + Area + url); ;
            httpReponse = await TryRefreshingTokenIfNeededAsync(() => HttpClient.GetAsync(BaseUrl + Area + url), httpReponse);
            return await HandleResponse<ResultType, string, Response<ResultType>>(httpReponse);
        }
        protected static string GenerateCollectionParametersUrl(int page, int pageSize, string[] orderBy, string singleSearch)
        {
            string parametersUrl = $"?page={page}&pageSize={pageSize}";

            if (orderBy != null && orderBy.Length > 0)
            {
                parametersUrl += $"&orderby={string.Join(',', orderBy)}";
            }
            if (!string.IsNullOrWhiteSpace(singleSearch))
            {
                parametersUrl += $"&singleSearch={HttpUtility.UrlEncode(singleSearch)}";
            }
            return parametersUrl;
        }
        private async Task<HttpResponseMessage> TryRefreshingTokenIfNeededAsync(Func<Task<HttpResponseMessage>> doRequest, HttpResponseMessage httpReponse)
        {
            if (httpReponse.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            {
                return httpReponse;
            }
            string token = await ContextProvider.GetRefreshTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return httpReponse;
            }

            AuthenticationClient authenticationClient = new AuthenticationClient(HttpClient, ContextProvider);
            var tokenResult = await authenticationClient.RefreshTokenAsync(token);
            if (tokenResult.Success)
            {
                await ContextProvider.SetToken(tokenResult.Data.Token);
                await ContextProvider.SetRefreshTokenAsync(tokenResult.Data.RefreshToken);
                httpReponse = await doRequest.Invoke();
            }
            return httpReponse;
        }


    }
}
