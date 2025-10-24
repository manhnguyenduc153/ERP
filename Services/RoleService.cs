using ERP_API.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists!" });

            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> UpdateRoleAsync(string oldName, string newName)
        {
            var role = await _roleManager.FindByNameAsync(oldName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found!" });

            role.Name = newName;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found!" });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<(bool Succeeded, string Message, AppUser User, IList<string> Roles)> AssignRolesAsync(string username, IList<string> roles)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return (false, "User does not exist", null, null);

            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(roles).ToList();

            IdentityResult resultAdd = IdentityResult.Success;
            IdentityResult resultRemove = IdentityResult.Success;

            if (rolesToAdd.Any())
                resultAdd = await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToRemove.Any())
                resultRemove = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!resultAdd.Succeeded || !resultRemove.Succeeded)
                return (false, "Failed to update roles", user, null);

            var updatedRoles = await _userManager.GetRolesAsync(user);
            return (true, "Roles updated successfully", user, updatedRoles);
        }
    }
}
