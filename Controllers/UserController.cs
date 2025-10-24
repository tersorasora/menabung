using Microsoft.AspNetCore.Mvc;
using Models;
using data;
using Microsoft.AspNetCore.Identity;
using Services;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var result = await _userService.RegisterUserAsync(user.email, user.nickname, user.password);
        if (!result.Success)
        {
            return BadRequest("Registration failed : " + result.Error);
        }
        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginRequest)
    {
        Console.WriteLine($"Login attempt for user: {loginRequest.email} {loginRequest.password}");
        var (user, message) = await _userService.LoginUserAsync(loginRequest.email, loginRequest.password);
        if (user == null)
        {
            return Unauthorized(message);
        }

        var token = _userService.GenerateJwtToken(user, _configuration);
        // Test
        // Return JSON with token and user info
        return Ok(new
        {
            token,
            user = new
            {
                id = user.user_id,
                nickname = user.nickname,
                role_id = user.role_id
            }
        });
    }


    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        if(users == null || users.Count == 0)
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpGet("balance/{id}")]
    public async Task<IActionResult> GetUserBalance(int id)
    {
        var balance = await _userService.GetUserBalanceAsync(id);
        return Ok(new { balance });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        var result = await _userService.UpdateUserAsync(user);
        if (!result)
        {
            return BadRequest("Failed to update user.");
        }
        return Ok("User updated successfully.");
    }

    [Authorize(Policy = "AdminOnly")] // Only admin can ban users
    [HttpPut("ban/{id}")]
    public async Task<IActionResult> BanUser(int id)
    {
        var result = await _userService.BanUserAsync(id);
        if (!result)
        {
            return BadRequest("Failed to ban user.");
        }
        return Ok("User banned successfully.");
    }

    [Authorize(Policy = "AdminOnly")] // Only admin can unban users
    [HttpPut("unban/{id}")]
    public async Task<IActionResult> UnbanUser(int id)
    {
        var result = await _userService.UnbanUserAsync(id);
        if (!result)
        {
            return BadRequest("Failed to unban user.");
        }
        return Ok("User unbanned successfully.");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return BadRequest("Failed to delete user.");
        }
        return Ok("User deleted successfully.");
    }

    [HttpGet("getRole/{id}")]
    public async Task<IActionResult> GetUserRole(int id)
    {
        var roleId = await _userService.GetUserRoleIdAsync(id);
        if (roleId == 0)
        {
            return NotFound("User role not found.");
        }
        return Ok(new { role_id = roleId });
    }

    [HttpGet("token")]
    public Task<IActionResult> GetTokenInfo()
    {
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim type: {claim.Type}, value: {claim.Value}");
        }

        return Task.FromResult<IActionResult>(Ok("Check console for token claims."));
    }
}