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
        catch(Exception ex)
        {
            Console.WriteLine("Error registering user: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAndPasswordAsync(username, password);
        return user != null;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<float> GetUserBalanceAsync(int userId)
    {
        return await _userRepository.GetUserBalanceAsync(userId);
    }
}