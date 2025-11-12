using ERP_API.Core.Database.Entities;
using ERP_API.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_API.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(string oldName, string newName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<(bool Succeeded, string Message, AppUser User, IList<string> Roles)> AssignRolesAsync(string username, IList<string> roles);
        Task<(bool Succeeded, string Message)> AssignPermissionsToRoleAsync(string roleName, List<Permission> permissions);
        List<object> GetAllPermissions();
        Task<List<object>> GetPermissionsByRoleAsync(string roleName);
    }
}
