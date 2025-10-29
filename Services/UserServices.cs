using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

using Models;
using Repositories;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<(bool Success, string? Error)> RegisterUserAsync(string username, string nickname, string password)
    {
        var user = new User
        {
            username = username,
            nickname = nickname,
            password = password,
            balance = 0,
            banned = false,
            role_id = 2 // Default role_id for regular members
        };

        user.password = HashPassword(user, password);

        try
        {
            await _userRepository.AddUser(user);
            return (true, "User registered successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error registering user: " + ex.Message);
            return (false, $"Error registering user: {ex.Message}");
        }
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public PasswordVerificationResult VerifyPassword(User user, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.password, providedPassword);
    }

    public async Task<(User?, string message)> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
        {
            return (null, "Username Not Registered");
        }

        var verificationResult = VerifyPassword(user, password);
        if(verificationResult != PasswordVerificationResult.Success)
        {
            return (null, "Invalid password");
        }

        if (user.banned)
        {
            return (null, "User is banned");
        }

        return (user, "Login successful");
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<int> GetUserRoleIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user?.role_id ?? 0;
    }

    public async Task<decimal> GetUserBalanceAsync(int userId)
    {
        return await _userRepository.GetUserBalanceAsync(userId);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.user_id);
            if(user.password != null && !user.password.Equals(existingUser?.password))
            {
                user.password = HashPassword(user, user.password);
            }
            await _userRepository.EditUser(user);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating user: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> BanUserAsync(int userId)
    {
        try
        {
            await _userRepository.BanUser(userId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error banning user: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> UnbanUserAsync(int userId)
    {
        try
        {
            await _userRepository.UnbanUser(userId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error banning user: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        return await _userRepository.DeleteUserAsync(userId);
    }
    
    public string GenerateJwtToken(User user, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.user_id.ToString()),
            new Claim(JwtRegisteredClaimNames.Nickname, user.nickname),
            new Claim("role_id", user.role_id.ToString())
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