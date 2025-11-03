using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<long> GetTotalRecord(AccountSearchModel search)
        {
            var query = _userManager.Users.AsQueryable();

            // Apply keyword filter
            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                var keyword = search.Keyword.ToLower().Trim();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(keyword) ||
                    (u.Email != null && u.Email.ToLower().Contains(keyword)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(keyword))
                );
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<AccountModel>> GetListPaging(AccountSearchModel search)
        {
            var query = _userManager.Users.AsQueryable();

            // Apply keyword filter
            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                var keyword = search.Keyword.ToLower().Trim();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(keyword) ||
                    (u.Email != null && u.Email.ToLower().Contains(keyword)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(keyword))
                );
            }

            // Apply pagination
            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .ToListAsync();

            // Build result with roles
            var result = new List<AccountModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new AccountModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return result;
        }
    }
}
