using System.ComponentModel;
using Models;

namespace Services;

public interface ITransactionServices
{
    Task<bool> AddTransactions(string description, string type, float nominal, int user_id);
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    Task<bool> EditTransaction(int transactionId, string description, string type, float nominal);
    Task<bool> DeleteTransaction(int transactionId);
}