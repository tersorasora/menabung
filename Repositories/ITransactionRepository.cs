using Models;

namespace Repositories;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task<List<Transaction>> GetAllTransactionsAsync();
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    Task<Transaction?> GetTransactionByIdAsync(int transactionId);
    Task<int> CountUserTransactionsAsync(int userId);
    Task EditTransaction(Transaction transaction);
    Task DeleteTransaction(int transactionId);
}