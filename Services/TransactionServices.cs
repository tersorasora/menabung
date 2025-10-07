using Models;
using Repositories;

namespace Services;

public class TransactionServices : ITransactionServices
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionServices(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<bool> AddTransactions(string description, string type, float nominal, int user_id)
    {
        var transaction = new Transaction
        {
            description = description,
            transaction_type = type,
            transaction_nominal = nominal,
            user_id = user_id,
            transaction_date = DateTime.UtcNow
        };

        try
        {
            await _transactionRepository.AddTransaction(transaction);
            return true;
        }catch (Exception ex)
        {
            Console.WriteLine($"Error adding transaction: {ex.Message}");
            return false;
        }
    }

    public Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
    {
        return _transactionRepository.GetTransactionsByUserIdAsync(userId);
    }
}