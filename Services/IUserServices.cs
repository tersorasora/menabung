using Models;

namespace Services;

public interface IUserService
{
    Task<bool> RegisterUserAsync(string username, string password);
    Task<User?> LoginUserAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<float> GetUserBalanceAsync(int userId);
    string GenerateJwtToken(User user, IConfiguration configuration);
}