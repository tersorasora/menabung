using System.ComponentModel;
using Models;

namespace Services;

public interface ITransactionServices
{
    Task<bool> AddTransactions(string description, string type, decimal nominal, DateTime date, int user_id);
    Task<List<Transaction>> GetAllTransactionsAsync();
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    Task<int> CountUserTransactionsAsync(int userId);
    Task<bool> EditTransaction(int transactionId, string description, string type, decimal nominal);
    Task<bool> DeleteTransaction(int transactionId);
}