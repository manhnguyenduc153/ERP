using ApiWithRoles.Models;
using ERP_API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithRoles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Đăng ký tài khoản mới
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var user = new AppUser { UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Register successfully!" });
            }

            return BadRequest(result.Errors);
        }

        // Đăng nhập không dùng JWT
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Nếu muốn trả thêm thông tin role:
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    message = "Login successfully!",
                    username = user.UserName,
                    roles = roles
                });
            }
            return Unauthorized(new { message = "Invalid username or password!" });
        }

        // Thêm role
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Role already exists!");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role));

            if (result.Succeeded)
            {
                return Ok(new { message = "Add role successfully!" });
            }

            return BadRequest(result.Errors);
        }

        // Gán role cho user
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return BadRequest("User does not exist!");
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Assign role successfully!" });
            }

            return BadRequest(result.Errors);
        }
    }
}
