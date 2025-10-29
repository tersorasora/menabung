using Models;
using System.Text;

namespace Services;

public interface IUserService
{
    Task<(bool Success, string? Error)> RegisterUserAsync(string username, string nickname, string password);
    Task<(User?, string message)> LoginUserAsync(string username, string password);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task<int> GetUserRoleIdAsync(int userId);
    Task<decimal> GetUserBalanceAsync(int userId);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> BanUserAsync(int userId);
    Task<bool> UnbanUserAsync(int userId);
    Task<bool> DeleteUserAsync(int userId);
    string GenerateJwtToken(User user, IConfiguration configuration);
}