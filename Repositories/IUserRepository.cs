using Models;

namespace Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task EditUser(User user);
    Task BanUser(int userId);
    Task UnbanUser(int userId);
    Task<bool> UpdateUserBalanceAsync(int userId, decimal newBalance);
    Task<bool> DeleteUserAsync(int userId);
    Task<decimal> GetUserBalanceAsync(int userId);
}