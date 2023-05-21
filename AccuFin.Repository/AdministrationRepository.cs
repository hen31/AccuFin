using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using AccuFin.Data.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return item.Map();
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

    }
}
