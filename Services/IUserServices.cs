using Models;

namespace Services;

public interface IUserService
{
    Task<(bool Success, string? Error)> RegisterUserAsync(string username, string nickname, string password);
    Task<User?> LoginUserAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<decimal> GetUserBalanceAsync(int userId);
    string GenerateJwtToken(User user, IConfiguration configuration);
}