using Models;
using data;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDBContext _context;

    public TransactionRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddTransaction(Transaction transaction)
    {
        _context.transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
    {
        return await _context.transactions
            .Where(t => t.user_id == userId)
            .ToListAsync();
    }
}