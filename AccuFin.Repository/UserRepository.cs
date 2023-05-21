using AccuFin.Api.Models;
using AccuFin.Api.Models.User;
using AccuFin.Data;
using AccuFin.Data.Entities;
using AccuFin.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AccuFin.Repository
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(AccuFinDatabaseContext databaseContext) : base(databaseContext)
        {
        }


        public async Task<CurrentUserModel> GetOrCreateCurrentUser(string emailAdress)
        {
            EntityRepository<AuthorizedUser, Guid> userRepository = new EntityRepository<AuthorizedUser, Guid>(DatabaseContext);
            var autherizedUser = await DatabaseContext.AuthorizedUsers.FirstOrDefaultAsync(b => b.EmailAdress == emailAdress);
            if (autherizedUser == null)
            {
                autherizedUser = new AuthorizedUser { EmailAdress = emailAdress };
                await userRepository.Add(autherizedUser);
                await DatabaseContext.SaveChangesAsync();
            }
            CurrentUserModel currentUserModel = MapAutherizedUserToModel(autherizedUser);
            return currentUserModel;
        }


        public async Task<CurrentUserModel> UpdateUserAsync(string emailAdress, CurrentUserModel userModel)
        {
            EntityRepository<AuthorizedUser, Guid> userRepository = new EntityRepository<AuthorizedUser, Guid>(DatabaseContext);
            var autherizedUser = await DatabaseContext.AuthorizedUsers.FirstOrDefaultAsync(b => b.EmailAdress == emailAdress);
            if (autherizedUser == null)
            {
                return null;
            }
            autherizedUser.Telephone = userModel.Telephone;
            autherizedUser.Name = userModel.Name;
            autherizedUser.MobileNumber = userModel.MobileNumber;
            await DatabaseContext.SaveChangesAsync();
            return MapAutherizedUserToModel(autherizedUser);
        }

        private static CurrentUserModel MapAutherizedUserToModel(AuthorizedUser autherizedUser)
        {
            CurrentUserModel currentUserModel = new CurrentUserModel();
            currentUserModel.Telephone = autherizedUser.Telephone;
            currentUserModel.MobileNumber = autherizedUser.MobileNumber;
            currentUserModel.Name = autherizedUser.Name;
            return currentUserModel;
        }

        public async Task<UserModel> GetUserByEmailAsync(string emailAdress)
        {
            var autherizedUser = await DatabaseContext.AuthorizedUsers.FirstOrDefaultAsync(b => b.EmailAdress == emailAdress);
            if (autherizedUser == null)
            {
                return null;
            }
            return autherizedUser.MapToModel();
        }
    }
}
