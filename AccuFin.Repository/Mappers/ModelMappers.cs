using AccuFin.Api.Models;
using AccuFin.Api.Models.BankIntegration;
using AccuFin.Api.Models.User;
using AccuFin.Data.Entities;

namespace AccuFin.Data.Mappers
{
    public static class ModelMappers
    {
        public static TransactionCollectionItem MapForCollection(this Data.Entities.Transaction entity)
        {
            return new TransactionCollectionItem()
            {

                Id = entity.Id,
                Description = entity.UnstructeredInformation,
                Amount = entity.Amount,
                IBAN = entity.FromIBAN,
                ToIBAN = entity.ToIBAN,
                TransactionDate = entity.TransactionDate
            };
        }
        public static AdministrationCollectionItem MapForCollection(this Administration entity)
        {
            return new AdministrationCollectionItem()
            {

                Id = entity.Id,
                Name = entity.Name,
                AdministrationRegistryCode = entity.AdministrationRegistryCode,
                ImageFileName = entity.HasImage ? entity.Id.ToString() + ".png" : null,
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
                ImageFileName = entity.HasImage ? entity.Id.ToString() + ".png" : null,
                TelephoneNumber = entity.TelephoneNumber
            };
        }

        public static Administration Map(this AdministrationModel model, Administration item)
        {
            item.Id = model.Id;
            item.Name = model.Name;
            item.AdministrationRegistryCode = model.AdministrationRegistryCode;
            item.HasImage = !string.IsNullOrWhiteSpace(model.ImageFileName);
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
                UserId = entity.UserId,
                Roles = ToRoles(entity.Roles)
            };
        }

        private static List<AdministrationRole> ToRoles(UserRoleInAdministration entityRoles)
        {
            List<AdministrationRole> roles = new List<AdministrationRole>();
            foreach (var role in AdministrationRole.GetRoles())
            {
                if (entityRoles.HasFlag((UserRoleInAdministration)role.Value))
                {
                    roles.Add(role);
                }
            }

            return roles;
        }

        public static UserModel MapToModel(this AuthorizedUser entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                Email = entity.EmailAdress,
                Name = entity.Name
            };
        }

        public static LinkBankAccountModel Map(this LinkBankAccount entity)
        {
            return new()
            {
                Id = entity.Id,
                IBAN = entity.IBAN,
                AccountId = entity.AccountId,
                Sync = entity.Sync
            };
        }


    }
}
