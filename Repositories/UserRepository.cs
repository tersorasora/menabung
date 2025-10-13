using Models;
using data;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDBContext _context;

    public UserRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        // check same username exists
        var existingUser = await _context.users.FirstOrDefaultAsync(u => u.username == user.username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists");
        }
        _context.users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.users.FindAsync(userId);
    }

    public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.username == username && u.password == password);
    }

    public async Task<bool> UpdateUserBalanceAsync(int userId, float newBalance)
    {
        var user = await _context.users.FindAsync(userId);
        if (user == null) return false;

        user.balance = newBalance;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<float> GetUserBalanceAsync(int userId)
    {
        var user = await _context.users.FindAsync(userId);
        return user?.balance ?? 0.0f;
    }
}