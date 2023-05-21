using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using AccuFin.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AccuFin.Repository
{
    public class AdministrationRepository : BaseRepository
    {
        public AdministrationRepository(AccuFinDatabaseContext databaseContext)
            : base(databaseContext)
        {
        }
        public async Task<AdministrationModel> AddItemAsync(AdministrationModel administration)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            var entity = administration.Map(new Administration());
            await administrationRepository.Add(entity);
            await DatabaseContext.SaveChangesAsync();
            administration = entity.Map();
            return administration;
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
            itemModel.Users = (await GetUsersForAdministrationMapped(item)).Map();
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
            await DatabaseContext.SaveChangesAsync();
            return item.Map();
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

        public async Task<FinCollection<AdministrationCollectionItem>> GetMyAdministrationsAsync(string searchText, AuthorizedUser authorizedUser)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            return null;
        }
    }
}
