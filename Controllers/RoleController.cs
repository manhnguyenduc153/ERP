using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Services;
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

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
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
            var (Succeeded, Message, User, Roles) = await _roleService.AssignRolesAsync(model.Username, model.Roles);

            if (!Succeeded)
                return BadRequest(new { data = (object?)null, message = Message, success = false, statusCode = 400 });

            return Ok(new
            {
                data = new
                {
                    userName = User.UserName,
                    email = User.Email,
                    phoneNumber = User.PhoneNumber,
                    roles = Roles
                },
                message = Message,
                success = true,
                statusCode = 200
            });
        }
    }
}
