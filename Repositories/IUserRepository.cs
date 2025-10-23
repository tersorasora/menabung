using Models;

namespace Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int userId);
    Task EditUser(User user);
    Task<bool> UpdateUserBalanceAsync(int userId, decimal newBalance);
    Task<bool> DeleteUserAsync(int userId);
    Task<decimal> GetUserBalanceAsync(int userId);
}