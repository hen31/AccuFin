using AccuFin.Api.Models;
using AccuFin.Api.Models.User;
using AccuFin.Data.Entities;

namespace AccuFin.Data.Mappers
{
    public static class ModelMappers
    {

        public static AdministrationCollectionItem MapForCollection(this Administration entity)
        {
            return new AdministrationCollectionItem()
            {

                Id = entity.Id,
                Name = entity.Name,
                AdministrationRegistryCode = entity.AdministrationRegistryCode
            };
        }
        public static AdministrationModel Map(this Administration entity)
        {
            return new AdministrationModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                AdministrationRegistryCode = entity.AdministrationRegistryCode,
                EmailAdress = entity.EmailAdress,
                TelephoneNumber = entity.TelephoneNumber
            };
        }

        public static Administration Map(this AdministrationModel model, Administration item)
        {
            item.Id = model.Id;
            item.Name = model.Name;
            item.AdministrationRegistryCode = model.AdministrationRegistryCode;
            item.EmailAdress = model.EmailAdress;
            item.TelephoneNumber = model.TelephoneNumber;
            return item;
        }

        public static UserAdministrationLinkModel Map(this UserAdministrationLink entity)
        {
            return new UserAdministrationLinkModel()
            {
                Id = entity.Id,
                Email = entity.User.EmailAdress,
                Name = entity.User.Name,
                Role = entity.Roles
            };
        }

    }
}
