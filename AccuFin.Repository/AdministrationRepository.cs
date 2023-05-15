using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
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
            var entity = Map(administration, new Administration());
            await administrationRepository.Add(entity);
            await DatabaseContext.SaveChangesAsync(); 
            administration = Map(entity, administration);
            return administration;
        }
        private AdministrationModel Map(Administration administration, AdministrationModel administrationModel)
        {
            administrationModel.Id = administration.Id;
            administrationModel.TelephoneNumber = administration.TelephoneNumber;
            administrationModel.EmailAdress = administration.EmailAdress;
            administrationModel.AdministrationRegistryCode = administration.AdministrationRegistryCode;
            administrationModel.Name = administration.Name;
            return administrationModel;
        }
        private Administration Map(AdministrationModel administrationModel, Administration administration)
        {
            administration.Id = administrationModel.Id;
            administration.TelephoneNumber = administrationModel.TelephoneNumber;
            administration.EmailAdress = administrationModel.EmailAdress;
            administration.AdministrationRegistryCode = administrationModel.AdministrationRegistryCode;
            administration.Name = administrationModel.Name;
            return administration;
        }

        public async Task<FinCollection<AdministrationCollectionItem>> GetCollectionAsync(int page, int pageSize)
        {
            EntityRepository<Administration, Guid> administrationRepository = new EntityRepository<Administration, Guid>(DatabaseContext);
            return (await administrationRepository.GetCollectionAsync(page, pageSize, b => true, b => new AdministrationCollectionItem()
            {
                Id = b.Id,
                Name = b.Name
            }));
        }


    }
}
