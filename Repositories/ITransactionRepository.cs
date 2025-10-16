using Models;

namespace Repositories;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    Task<Transaction?> GetTransactionByIdAsync(int transactionId);
    Task EditTransaction(Transaction transaction);
    Task DeleteTransaction(int transactionId);
}