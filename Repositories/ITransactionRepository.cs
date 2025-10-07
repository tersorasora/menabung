using Models;

namespace Repositories;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
}