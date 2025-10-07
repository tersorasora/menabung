using System.ComponentModel;
using Models;

namespace Services;

public interface ITransactionServices
{
    Task<bool> AddTransactions(string description, string type, float nominal, int user_id);
    Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
}