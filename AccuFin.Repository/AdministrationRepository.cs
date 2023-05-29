using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using AccuFin.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AccuFin.Repository
{
    public class AdministrationRepository : BaseRepository
    {
        private readonly BankIntegrationRepository _bankIntegrationRepository;

        public AdministrationRepository(AccuFinDatabaseContext databaseContext, BankIntegrationRepository bankIntegrationRepository)
            : base(databaseContext)
        {
            _bankIntegrationRepository = bankIntegrationRepository;
        }
        public async Task<AdministrationModel> AddItemAsync(AdministrationModel administration)
        {
            administration.Id = Guid.Empty;
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            var entity = administration.Map(new Administration());
            await administrationRepository.Add(entity);


            await UpdateOrAddUsersToAdministration(entity, administration.Users);

            var users = administration.Users;
            administration = entity.Map();
            await DatabaseContext.SaveChangesAsync();
            administration.Users = (await GetUsersForAdministrationMapped(entity)).Select(b => b.Map()).ToList();
            return administration;
        }

        private async Task UpdateOrAddUsersToAdministration(Administration entity, List<UserAdministrationLinkModel> links)
        {
            EntityRepository<UserAdministrationLink, Guid> administrationLinkRepository = new EntityRepository<UserAdministrationLink, Guid>(DatabaseContext);
            List<UserAdministrationLink> currentUsersOfAdministration = new List<UserAdministrationLink>();
            if (entity.Id != Guid.Empty)
            {
                currentUsersOfAdministration = DatabaseContext.UserAdministrationLink.Where(b => b.AdministrationId == entity.Id).Include(b => b.User).ToList();
            }
            foreach (var userLink in links)
            {
                if (userLink.Id == Guid.Empty
                    && !currentUsersOfAdministration.Any(b => b.UserId == userLink.Id))
                {
                    var newLink = new UserAdministrationLink()
                    {
                        UserId = userLink.UserId,
                        AdministrationId = entity.Id,
                        Roles = (UserRoleInAdministration)userLink.Roles.Sum(b => b.Value)
                    };
                    if (entity.Id == Guid.Empty)
                    {
                        newLink.Administration = entity;
                    }
                    await administrationLinkRepository.Add(newLink);
                    continue;
                }
                var userLinkFromDb = currentUsersOfAdministration.SingleOrDefault(b => b.UserId == userLink.UserId);
                userLinkFromDb.Roles = (UserRoleInAdministration)userLink.Roles.Sum(b => b.Value);
                administrationLinkRepository.Update(userLinkFromDb);
            }
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task<FinCollection<AdministrationCollectionItem>> GetCollectionAsync(int page, int pageSize, string[] orderBy, string singleSearchText)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            return await administrationRepository.GetCollectionAsync(page, pageSize, orderBy,
                b => EF.Functions.Like(b.Name, $"%{singleSearchText}%") || EF.Functions.Like(b.AdministrationRegistryCode, $"{singleSearchText}%"),
                b => b.MapForCollection());
        }

        public async Task<AdministrationModel> GetItemByIdAsync(Guid id)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            var item = await administrationRepository.GetById(id);
            if (item == null)
            {
                return null;
            }
            var itemModel = item.Map();
            itemModel.Users = (await GetUsersForAdministrationMapped(item)).Select(b => b.Map()).ToList();
            itemModel.BankAccounts = (await _bankIntegrationRepository.GetLinkedBankAccountsAsync(item.Id));
            return itemModel;
        }

        public async Task<List<UserAdministrationLink>> GetUsersForAdministrationMapped(Administration administration)
        {
            return await DatabaseContext.UserAdministrationLink.Where(b => b.AdministrationId == administration.Id).Include(b => b.User).ToListAsync();
        }

        public async Task<AdministrationModel> EditItemAsync(Guid id, AdministrationModel model)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            var item = await administrationRepository.GetById(id);
            if (item == null) { return null; }
            item = model.Map(item);
            administrationRepository.Update(item);
            await _bankIntegrationRepository.SyncBankAccountsAsync(id, model.BankAccounts);
            await UpdateOrAddUsersToAdministration(item, model.Users);
            var mappedItem = item.Map();
            await DatabaseContext.SaveChangesAsync();
            mappedItem.Users = (await GetUsersForAdministrationMapped(item)).Select(b => b.Map()).ToList();
            return mappedItem;
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            var item = await administrationRepository.GetById(id);
            if (item == null) { return false; }
            administrationRepository.Delete(item);
            await DatabaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AdministrationCollectionItem>> GetMyAdministrationsAsync(Guid authorizedUserId)
        {
            return (await DatabaseContext.UserAdministrationLink.Where(b => b.UserId == authorizedUserId).Include(b => b.Administration).Select(b => b.Administration).ToListAsync()).Select(b => b.MapForCollection());
        }

    }
}
