using AccuFin.Api.Models.BankIntegration;
using AccuFin.Api.Services.BankIntegration;
using AccuFin.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;

namespace AccuFin.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("bankintegration")]
    public class BankIntegrationController : ControllerBase
    {
        private readonly BankIntegrationRepository _bankIntegrationRepository;
        private readonly UserRepository _userRepository;
        private readonly NordigenClient _nordigenClient;
        private readonly AdministrationRepository _administrationRepository;
        public BankIntegrationController(BankIntegrationRepository bankIntegrationRepository,
            UserRepository userRepository,
            AdministrationRepository administrationRepository,
            NordigenClient nordigenClient)
        {
            _bankIntegrationRepository = bankIntegrationRepository;
            _userRepository = userRepository;
            _nordigenClient = nordigenClient;
            _administrationRepository = administrationRepository;
        }

        [HttpGet("banks")]
        [Authorize()]
        public async Task<ActionResult<ICollection<BankModel>>> FindBank()
        {
            return Ok(await _nordigenClient.GetBanksAsync());
        }

        [HttpGet("getlink/{institution}/{administrationId}")]
        [Authorize()]
        public async Task<ActionResult<BankLinkAuthorizationModel>> GetLink(string institution, Guid administrationId)
        {
            if (await this.IsUserAuthorizedForAdministration(_administrationRepository,
                administrationId) == false)
            {
                return BadRequest();
            }
            var bankLink = await _nordigenClient.GenerateBankLinkAsyc($"{GetBaseUrl()}/bankintegration/done", institution);
            await _bankIntegrationRepository.AddIntegration(this.GetFinUserId(), administrationId, institution, bankLink.Link, bankLink.Reference, this.GetClientId(), bankLink.Id);
            return Ok(bankLink);
        }

        [HttpGet("getaccountinformation/{administrationId}")]
        [Authorize()]
        public async Task<ActionResult<ICollection<ExternalBankAccountModel>>> GetAccountsForAdministrationAsync(Guid administrationId)
        {
            if (await this.IsUserAuthorizedForAdministration(_administrationRepository,
               administrationId) == false)
            {
                return BadRequest();
            }
            List<ExternalBankAccountModel> results = new List<ExternalBankAccountModel>();
            var integrations = await _bankIntegrationRepository.GetActiveIntegrationsForAdministrationAsync(administrationId);
            foreach (var integration in integrations)
            {
                var accounts = await _nordigenClient.GetBankAccountsInfoAsync(integration.ExternalLinkId);
                foreach (var account in accounts.Accounts)
                {
                    var accountInfo = await _nordigenClient.GetAcountInfoAsync(account);
                    ExternalBankAccountModel externalBankAccountModel = new ExternalBankAccountModel();
                    externalBankAccountModel.ExternalAccountId = integration.ExternalLinkId;
                    externalBankAccountModel.AccountId = accountInfo.Id;
                    externalBankAccountModel.IBAN = accountInfo.Iban;
                    externalBankAccountModel.Name = accountInfo.Owner_name.Replace("CJ", "/");
                    results.Add(externalBankAccountModel);
                }
            }
            return Ok(results);
        }

        public string GetBaseUrl()
        {
            var request = HttpContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }


        [HttpGet("done")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> IntegrationAuthorized([FromQuery(Name = "ref")] string linkReference)
        {
            var integration = await _bankIntegrationRepository.GetIntegrationAsync(linkReference);
            await _bankIntegrationRepository.SetAuthorized(integration);

            var integrations = await _bankIntegrationRepository.GetActiveIntegrationsForAdministrationAsync(integration.AdministrationId);
            foreach (var currentIntegration in integrations)
            {
                var accounts = await _nordigenClient.GetBankAccountsInfoAsync(integration.ExternalLinkId);
                foreach (var account in accounts.Accounts)
                {
                    var accountInfo = await _nordigenClient.GetAcountInfoAsync(account);
                    await _bankIntegrationRepository.LinkBankAccountAsync(integration.AdministrationId, accountInfo.Id, accountInfo.Iban);
                }
            }

            if (integration.ClientId == "AccuFin.Online")
            {
                return Redirect($"https://localhost:7266/administration/{integration.AdministrationId}");
            }
            return Ok("true");
        }
    }
}