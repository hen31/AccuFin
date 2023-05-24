using AccuFin.Api.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace AccuFin.Api.Services
{
    public class RoleService
    {
        private readonly UserManager<AccuFinUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AccuFinUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
    }

        private async Task AddRoles()
        {
            if (await _roleManager.RoleExistsAsync(Roles.Administrator) == false)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            }
            if (await _roleManager.RoleExistsAsync(Roles.Owner) == false)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Owner));
            }
        }

        public async Task AddRoleToUser(AccuFinUser user, string role)
        {
            await AddRoles();
            await _userManager.AddToRoleAsync(user, role);
        }


    }
}
