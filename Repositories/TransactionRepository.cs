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

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _context.transactions.OrderBy(t => t.transaction_id)
            .Include(t => t.user)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
    {
        return await _context.transactions
            .Include(t => t.user)
            .Where(t => t.user_id == userId)
            .OrderBy(t => t.transaction_id)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
    {
        return await _context.transactions.FindAsync(transactionId);
    }

    public async Task<int> CountUserTransactionsAsync(int userId)
    {
        return await _context.transactions.CountAsync(t => t.user_id == userId);
    }

    public async Task EditTransaction(Transaction transaction)
    {
        _context.transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(int transactionId)
    {
        var transaction = await _context.transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            _context.transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}