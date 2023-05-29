using AccuFin.Api.Models;
using AccuFin.Data.Entities;
using AccuFin.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuFin.Api.Models.BankIntegration;
using AccuFin.Data.Mappers;

namespace AccuFin.Repository
{
    public class BankIntegrationRepository : BaseRepository
    {
        public BankIntegrationRepository(AccuFinDatabaseContext databaseContext) : base(databaseContext)
        {
        }


        public async Task<bool> AddIntegration(Guid userId, Guid administrationId, string institution, string link, string reference, string clientId, string bankLinkId)
        {
            EntityRepository<BankIntegration, Guid> bankIntegrationRepository = new EntityRepository<BankIntegration, Guid>(DatabaseContext);
            BankIntegration bankIntegration = new BankIntegration();
            bankIntegration.UserId = userId;
            bankIntegration.AdministrationId = administrationId;
            bankIntegration.InitializedOn = DateTime.UtcNow;
            bankIntegration.Institution = institution;
            bankIntegration.Link = link;
            bankIntegration.ClientId = clientId;
            bankIntegration.Reference = reference;
            bankIntegration.ExternalLinkId = bankLinkId;
            await bankIntegrationRepository.Add(bankIntegration);
            await DatabaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<BankIntegration> GetIntegrationAsync(string linkReference)
        {
            return await DatabaseContext.BankIntegrations.FirstOrDefaultAsync(b => b.Reference == linkReference);

        }
        public async Task<List<BankIntegration>> GetActiveIntegrationsForAdministrationAsync(Guid administrationId)
        {
            return await DatabaseContext.BankIntegrations.Where(b => b.AdministrationId == administrationId && b.Accepted).ToListAsync();

        }
        public async Task SetAuthorized(BankIntegration integration)
        {
            EntityRepository<BankIntegration, Guid> bankIntegrationRepository = new EntityRepository<BankIntegration, Guid>(DatabaseContext);
            integration.Accepted = true;
            integration.AcceptedOn = DateTime.UtcNow;
            bankIntegrationRepository.Update(integration);
            await DatabaseContext.SaveChangesAsync();
        }


        public async Task<LinkBankAccount> LinkBankAccountAsync(Guid administrationId, string accountId, string iban)
        {
            EntityRepository<LinkBankAccount, Guid> linkBankAccountRepository = new EntityRepository<LinkBankAccount, Guid>(DatabaseContext);
            if (await DatabaseContext.LinkBankAccounts.AnyAsync(b => b.IBAN == iban && b.AdministrationId == administrationId))
            {
                return null;
            }
            LinkBankAccount linkBank = new LinkBankAccount();
            linkBank.AdministrationId = administrationId;
            linkBank.AccountId = accountId;
            linkBank.IBAN = iban;
            linkBank.Sync = false;
            await linkBankAccountRepository.Add(linkBank);
            await DatabaseContext.SaveChangesAsync();
            return linkBank;
        }


        public async Task<List<LinkBankAccountModel>> SyncBankAccountsAsync(Guid administrationId, List<LinkBankAccountModel> accounts)
        {
            EntityRepository<LinkBankAccount, Guid> linkBankAccountRepository = new EntityRepository<LinkBankAccount, Guid>(DatabaseContext);
            foreach (var item in accounts)
            {
                var linkAccount = await linkBankAccountRepository.GetById(item.Id);
                if (linkAccount != null)
                {
                    linkAccount.Sync = item.Sync;
                    linkBankAccountRepository.Update(linkAccount);
                }
            }
            await DatabaseContext.SaveChangesAsync();
            return await GetLinkedBankAccountsAsync(administrationId);
        }

        public async Task<List<LinkBankAccountModel>> GetLinkedBankAccountsAsync(Guid administrationId)
        {
            return (await DatabaseContext.LinkBankAccounts.Where(b => b.AdministrationId == administrationId).ToListAsync()).Select(b => b.Map()).ToList();
        }
    }
}
