using ERP_API.Authorization;
using ERP_API.Entities;
using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoleController(IRoleService roleService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleService = roleService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var result = roles.Select(r => new { r.Id, r.Name }).ToList();

            return Ok(new
            {
                data = result,
                message = "Get roles successfully!",
                success = true,
                statusCode = 200
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            var result = await _roleService.AddRoleAsync(roleName);
            if (!result.Succeeded)
                return BadRequest(new { data = (object?)null, message = result.Errors.First().Description, success = false, statusCode = 400 });

            return Ok(new { data = new { name = roleName }, message = "Add role successfully!", success = true, statusCode = 200 });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateModel model)
        {
            var result = await _roleService.UpdateRoleAsync(model.OldName, model.NewName);
            if (!result.Succeeded)
                return BadRequest(new { data = (object?)null, message = result.Errors.First().Description, success = false, statusCode = 400 });

            return Ok(new { data = new { name = model.NewName }, message = "Role updated successfully!", success = true, statusCode = 200 });
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRoleAsync(roleName);
            if (!result.Succeeded)
                return BadRequest(new { data = (object?)null, message = result.Errors.First().Description, success = false, statusCode = 400 });

            return Ok(new { data = (object?)null, message = "Role deleted successfully!", success = true, statusCode = 200 });
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var (Succeeded, Message, UpdatedUser, Roles) = await _roleService.AssignRolesAsync(model.Username, model.Roles);

            if (!Succeeded)
                return BadRequest(new { data = (object?)null, message = Message, success = false, statusCode = 400 });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && currentUser.UserName == model.Username)
            {
                await _userManager.UpdateSecurityStampAsync(currentUser);
                await _signInManager.RefreshSignInAsync(currentUser);
            }

            return Ok(new
            {
                data = new
                {
                    userName = UpdatedUser.UserName,
                    email = UpdatedUser.Email,
                    phoneNumber = UpdatedUser.PhoneNumber,
                    roles = Roles
                },
                message = Message,
                success = true,
                statusCode = 200
            });
        }

        [HttpPost("assign-permissions")]
        public async Task<IActionResult> AssignPermissions([FromBody] RolePermissionModel model)
        {
            var result = await _roleService.AssignPermissionsToRoleAsync(model.RoleName, model.Permissions);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Message, success = false, statusCode = 400 });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && await _userManager.IsInRoleAsync(currentUser, model.RoleName))
            {
                await _userManager.UpdateSecurityStampAsync(currentUser);
                await _signInManager.RefreshSignInAsync(currentUser);
            }


            return Ok(new { message = result.Message, success = true, statusCode = 200 });
        }

        [HttpGet("permissions")]
        public IActionResult GetAllPermissions()
        {
            var permissions = _roleService.GetAllPermissions();
            return Ok(new
            {
                data = permissions,
                message = "Get all permissions successfully!",
                success = true,
                statusCode = 200
            });
        }

        [HttpGet("{roleName}/permissions")]
        public async Task<IActionResult> GetPermissionsByRole(string roleName)
        {
            var permissions = await _roleService.GetPermissionsByRoleAsync(roleName);
            if (permissions == null)
                return NotFound(new
                {
                    data = (object?)null,
                    message = "Role not found!",
                    success = false,
                    statusCode = 404
                });

            return Ok(new
            {
                data = permissions,
                message = "Get role permissions successfully!",
                success = true,
                statusCode = 200
            });
        }

    }
}
