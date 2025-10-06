using System.Collections;
using Models;

namespace Services;

public interface IUserService
{
    Task<bool> RegisterUserAsync(User user);
    Task<bool> LoginUserAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId); 
    Task<float> GetUserBalanceAsync(int userId);
}