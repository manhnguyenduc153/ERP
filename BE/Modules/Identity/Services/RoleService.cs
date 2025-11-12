using ERP_API.Core.Database.Entities;
using ERP_API.Enums;
using ERP_API.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<(bool Succeeded, string Message)> AssignPermissionsToRoleAsync(string roleName, List<Permission> permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return (false, "Role not found!");

            var existingClaims = await _roleManager.GetClaimsAsync(role);

            // Xóa các claim Permission cũ
            foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            // Thêm các Permission mới
            foreach (var permission in permissions)
            {
                var claim = new Claim("Permission", permission.ToString());
                await _roleManager.AddClaimAsync(role, claim);
            }

            return (true, "Permissions assigned successfully!");
        }

        public List<object> GetAllPermissions()
        {
            return System.Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(p => new
                {
                    Name = p.ToString(),
                    Description = p.GetDescription()
                })
                .ToList<object>();
        }

        // 🧩 Lấy Permission của 1 Role cụ thể
        public async Task<List<object>> GetPermissionsByRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return null;

            var claims = await _roleManager.GetClaimsAsync(role);

            return claims
                .Where(c => c.Type == "Permission")
                .Select(c => new
                {
                    Name = c.Value,
                    Description = ((Permission)System.Enum.Parse(typeof(Permission), c.Value)).GetDescription()
                })
                .ToList<object>();
        }
    }
}
