using ERP_API.Entities;
using ERP_API.Models;
using Microsoft.AspNetCore.Identity;

namespace ERP_API.Services.IServices
{
    public interface IAccountService
    {
        Task<ResponseData<IEnumerable<AccountModel>>> GetListPaging(AccountSearchModel search);
        Task<(bool Success, IEnumerable<IdentityError>? Errors)> RegisterAsync(Register model);
        Task<(bool Success, AppUser? User, IEnumerable<string>? Roles)> LoginAsync(Login model);
        Task<IEnumerable<object>> GetUsersWithRolesAsync();
        Task LogoutAsync();
        Task<(bool Success, IEnumerable<IdentityError>? Errors)> AddRoleAsync(string role);
        Task<(bool Success, AppUser? User, IEnumerable<string>? Roles)> AssignRoleAsync(UserRole model);
        Task<AppUser?> GetCurrentUserAsync(string username);
        Task<(bool Success, IEnumerable<IdentityError>? Errors)> UpdateAccountAsync(UpdateAccountModel model);
        Task<(bool Success, IEnumerable<IdentityError>? Errors)> ChangePasswordAsync(ChangePasswordModel model);
        Task<(bool Success, IEnumerable<IdentityError>? Errors)> DeleteAccountAsync(string username);
    }
}
