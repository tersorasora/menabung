using Microsoft.AspNetCore.Mvc;
using Models;
using data;
using Microsoft.AspNetCore.Identity;
using Services;

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
        var result = await _userService.RegisterUserAsync(user.username, user.nickname, user.password);
        if (!result)
        {
            return BadRequest("Registration failed.");
        }
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginRequest)
    {
        var user = await _userService.LoginUserAsync(loginRequest.username, loginRequest.password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _userService.GenerateJwtToken(user, _configuration);
        // Return JSON with token and user info
        return Ok(new 
        { 
            token, 
            user = new 
            {
                id = user.user_id,
                nickname = user.nickname
            } 
        });
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