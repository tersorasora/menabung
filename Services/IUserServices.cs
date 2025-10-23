using Models;
using System.Text;

namespace Services;

public interface IUserService
{
    Task<(bool Success, string? Error)> RegisterUserAsync(string email, string nickname, string password);
    Task<(User?, string message)> LoginUserAsync(string email, string password);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task<int> GetUserRoleIdAsync(int userId);
    Task<decimal> GetUserBalanceAsync(int userId);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userId);
    string GenerateJwtToken(User user, IConfiguration configuration);
}