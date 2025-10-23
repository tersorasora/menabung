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
        // check same emai exists
        var existingUser = await _context.users.FirstOrDefaultAsync(u => u.email == user.email);
        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }
        _context.users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.users.FindAsync(userId);
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.email == email && u.password == password);
    }

    public async Task EditUser(User user)
    {
        var existingUser = await _context.users.FirstOrDefaultAsync(u => u.user_id == user.user_id);

        if (existingUser == null)
            throw new Exception("User not found");

        // Update only the properties you allow
        existingUser.nickname = user.nickname;
        existingUser.password = user.password;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateUserBalanceAsync(int userId, decimal newBalance)
    {
        var user = await _context.users.FindAsync(userId);
        if (user == null) return false;

        user.balance = newBalance;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.users.FindAsync(userId);
        if (user == null) return false;

        _context.users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetUserBalanceAsync(int userId)
    {
        var user = await _context.users.FindAsync(userId);
        return user?.balance ?? 0;
    }
}