using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using AccuFin.Api.Models.BankIntegration;
using NuGet.Common;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;

namespace AccuFin.Api.Services.BankIntegration
{
    public class NordigenClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private SecretResult _currentToken;

        public NordigenClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        internal async Task<BankLinkAuthorizationModel> GenerateBankLinkAsyc(string returnUrl, string institution)
        {
            //institution = "SANDBOXFINANCE_SFIN0000";
            //Generate agreement
            //, max_historical_days = 720, access_valid_for_days = 365 


            string agreementId = await GetAgreementIdAsync(institution);
            var httpRequest = await GetAutherizedHttpRequest("https://ob.nordigen.com/api/v2/requisitions/");
            httpRequest.Method = HttpMethod.Post;

            string referenceId = Guid.NewGuid().ToString();
            string jsonContent = JsonConvert.SerializeObject(new { redirect = returnUrl, institution_id = institution, reference = referenceId, agreement = agreementId });
            httpRequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient.SendAsync(httpRequest);
            var bankLink = JsonConvert.DeserializeObject<BankLinkAuthorizationModel>(await result.Content.ReadAsStringAsync());
            return bankLink;
        }


        private async Task<string> GetAgreementIdAsync(string institution)
        {
            HttpRequestMessage httpRequest = await GetAutherizedHttpRequest("https://ob.nordigen.com/api/v2/agreements/enduser/");
            httpRequest.Method = HttpMethod.Post;
            string jsonContent = JsonConvert.SerializeObject(new { institution_id = institution, max_historical_days = 720, access_valid_for_days = 90, access_scope = new string[] { "balances", "details", "transactions" } });
            httpRequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient.SendAsync(httpRequest);
            string resultAsJson = await result.Content.ReadAsStringAsync();
            return (await result.Content.ReadFromJsonAsync<AgreementResultModel>()).Id;

        }

        private async Task<HttpRequestMessage> GetAutherizedHttpRequest(string url, bool accept = true)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.RequestUri = new Uri(url);
            if (accept)
            {
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            SecretResult token = await GetTokenForNordigen();
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Access);
            return httpRequest;
        }

        internal async Task<AccountInfoResultModel> GetAcountInfoAsync(string accountId)
        {
            HttpClient httpClient = await GetClientForAutherizedRequest();
            var result = await httpClient.GetAsync($"https://ob.nordigen.com/api/v2/accounts/{accountId}/");
            return await result.Content.ReadFromJsonAsync<AccountInfoResultModel>();
        }

        internal async Task<TransactionsResultModel> GetTransactionsAsync(string accountId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();

            string url = $"https://ob.nordigen.com/api/v2/accounts/{accountId}/transactions/";
            var request = await GetAutherizedHttpRequest(url, false);
            request.Method = HttpMethod.Get;

            var result = await httpClient.SendAsync(request);
            // string json =  await result.Content.ReadAsStringAsync();
            return await result.Content.ReadFromJsonAsync<TransactionsResultModel>();
        }


        internal async Task<ICollection<BankModel>> GetBanksAsync()
        {
            HttpClient httpClient = await GetClientForAutherizedRequest();
            var result = await httpClient.GetAsync("https://ob.nordigen.com/api/v2/institutions/?country=nl");
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<BankModel>>(json);
        }
        internal async Task<ListBankAccountResultModel> GetBankAccountsInfoAsync(string accountId)
        {
            //        HttpClient httpClient = await GetClientForAutherizedRequest();
            //var result = await httpClient.GetAsync($"https://ob.nordigen.com/api/v2/requisitions/{accountId}");
            var request = await GetAutherizedHttpRequest($"https://ob.nordigen.com/api/v2/requisitions/{accountId}/", false);
            request.Method = HttpMethod.Get;
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Clear();
            var result = await httpClient.SendAsync(request);
            return await result.Content.ReadFromJsonAsync<ListBankAccountResultModel>();
        }
        private async Task<HttpClient> GetClientForAutherizedRequest()
        {
            var token = await GetTokenForNordigen();
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        internal async Task<SecretResult> GetTokenForNordigen()
        {
            if (_currentToken == null)
            {
                var httpClient = _httpClientFactory.CreateClient();
                HttpRequestMessage httpRequest = new HttpRequestMessage();
                httpRequest.Method = HttpMethod.Post;
                httpRequest.RequestUri = new Uri("https://ob.nordigen.com/api/v2/token/new/");
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string jsonContent = JsonConvert.SerializeObject(new { secret_id = _configuration["nordigen:secret_id"], secret_key = _configuration["nordigen:secret_key"] });
                httpRequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var result = await httpClient.SendAsync(httpRequest);
                _currentToken = await result.Content.ReadFromJsonAsync<SecretResult>();
            }
            return _currentToken;
        }
    }
}
