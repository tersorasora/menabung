using Models;

namespace Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> UpdateUserBalanceAsync(int userId, float newBalance);
    Task<float> GetUserBalanceAsync(int userId);
}