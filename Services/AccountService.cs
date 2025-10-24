using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace ERP_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> RegisterAsync(Register model)
        {
            var user = new AppUser
            {
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return (result.Succeeded, result.Errors);
        }

        public async Task<(bool Success, AppUser? User, IEnumerable<string>? Roles)> LoginAsync(Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                var roles = await _userManager.GetRolesAsync(user);
                return (true, user, roles);
            }
            return (false, null, null);
        }

        public async Task<IEnumerable<object>> GetUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();
            var list = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    Roles = roles
                });
            }

            return list;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> AddRoleAsync(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
                return (false, null);

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            return (result.Succeeded, result.Errors);
        }

        public async Task<(bool Success, AppUser? User, IEnumerable<string>? Roles)> AssignRoleAsync(UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (false, null, null);

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(model.Roles).ToList();

            IdentityResult resultAdd = IdentityResult.Success;
            IdentityResult resultRemove = IdentityResult.Success;

            if (rolesToAdd.Any())
                resultAdd = await _userManager.AddToRolesAsync(user, rolesToAdd);

            if (rolesToRemove.Any())
                resultRemove = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            if (!resultAdd.Succeeded || !resultRemove.Succeeded)
                return (false, user, null);

            var updatedRoles = await _userManager.GetRolesAsync(user);
            return (true, user, updatedRoles);
        }

        public async Task<AppUser?> GetCurrentUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> UpdateAccountAsync(UpdateAccountModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) return (false, null);

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.SecurityStamp = Guid.NewGuid().ToString();

            var result = await _userManager.UpdateAsync(user);
            return (result.Succeeded, result.Errors);
        }

        public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) return (false, null);

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return (result.Succeeded, result.Errors);
        }

        public async Task<(bool Success, IEnumerable<IdentityError>? Errors)> DeleteAccountAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return (false, null);

            var result = await _userManager.DeleteAsync(user);
            return (result.Succeeded, result.Errors);
        }

        public async Task<ResponseData<IEnumerable<AccountModel>>> GetListPaging(AccountSearchModel search)
        {
            try
            {
                var totalRecord = await _accountRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _accountRepository.GetListPaging(search);
                    var pagedList = new PagedList<AccountModel>(list, totalRecord, search.PageIndex, search.PageSize);
                    return new ResponseData<IEnumerable<AccountModel>>(true, pagedList, pagedList.GetMetaData());
                }
                return new ResponseData<IEnumerable<AccountModel>>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<IEnumerable<AccountModel>>(ex.Message);
            }
        }
    }
}
