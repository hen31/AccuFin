using AccuFin.Api.Models;
using AccuFin.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
