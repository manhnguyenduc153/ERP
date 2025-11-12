using ERP_API.Models;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Register model)
    {
        var (success, errors) = await _accountService.RegisterAsync(model);
        if (success)
            return Ok(new { message = "Register successfully!", success = true, statusCode = 200 });

        return BadRequest(new { data = errors, message = "Register failed!", success = false, statusCode = 400 });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
        var (success, user, roles, permissions) = await _accountService.LoginAsync(model);
        if (success)
            return Ok(new
            {
                message = "Login successfully!",
                username = user!.UserName,
                roles,
                permissions
            });

        return Unauthorized(new { message = "Invalid username or password!" });
    }

    [HttpGet("GetUsersWithRoles")]
    public async Task<IActionResult> GetUsersWithRoles([FromQuery] AccountSearchModel search)
    {
        //var data = await _accountService.GetUsersWithRolesAsync();
        //return Ok(new { data, message = "Get users with roles successfully!", success = true, statusCode = 200 });
        var result = await _accountService.GetListPaging(search);
        return Ok(result);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok(new { message = "Logout successfully!" });
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] string role)
    {
        var (success, errors) = await _accountService.AddRoleAsync(role);
        if (success) return Ok(new { message = "Add role successfully!" });
        return BadRequest(new { data = errors, message = "Add role failed!" });
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] UserRole model)
    {
        var (success, user, roles) = await _accountService.AssignRoleAsync(model);
        if (!success) return BadRequest(new { message = "Failed to update roles!" });

        return Ok(new
        {
            data = new { user!.UserName, user.Email, user.PhoneNumber, roles },
            message = "Roles updated successfully!",
            success = true,
            statusCode = 200
        });
    }

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountModel model)
    {
        var (success, errors) = await _accountService.UpdateAccountAsync(model);
        if (success) return Ok(new { message = "Update account successfully!", success = true, statusCode = 200 });
        return BadRequest(new { data = errors, message = "Update account failed!", success = false, statusCode = 400 });
    }

    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        var (success, errors) = await _accountService.ChangePasswordAsync(model);
        if (success) return Ok(new { message = "Password changed successfully!", success = true, statusCode = 200 });
        return BadRequest(new { data = errors, message = "Change password failed!", success = false, statusCode = 400 });
    }

    [HttpDelete("DeleteAccount/{username}")]
    public async Task<IActionResult> DeleteAccount(string username)
    {
        var (success, errors) = await _accountService.DeleteAccountAsync(username);
        if (success) return Ok(new { message = "Account deleted successfully!", success = true, statusCode = 200 });
        return BadRequest(new { data = errors, message = "Delete account failed!", success = false, statusCode = 400 });
    }

    [HttpGet("CurrentUser")]
    public async Task<IActionResult> CurrentUser()
    {
        if (User?.Identity == null || !User.Identity.IsAuthenticated)
            return Unauthorized(new { message = "Not authenticated" });

        var username = User.Identity.Name;
        var user = await _accountService.GetCurrentUserAsync(username!);
        if (user == null) return NotFound(new { message = "User not found" });

        var roles = await _accountService.LoginAsync(new Login { Username = user.UserName, Password = "" });
        return Ok(new
        {
            username = user.UserName,
            email = user.Email,
            phoneNumber = user.PhoneNumber,
            roles = roles.Roles
        });
    }
}
