using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Models;
using Repositories;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        var user = new User
        {
            username = username,
            nickname = username,
            password = password,
            balance = 0.0f
        };

        try
        {
            await _userRepository.AddUser(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error registering user: " + ex.Message);
            return false;
        }
    }

    public async Task<User?> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAndPasswordAsync(username, password);
        if (user != null)
        {
            return user;
        }
        return null;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<float> GetUserBalanceAsync(int userId)
    {
        return await _userRepository.GetUserBalanceAsync(userId);
    }
    
    public string GenerateJwtToken(User user, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.user_id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}