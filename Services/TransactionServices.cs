using Models;
using Repositories;

namespace Services;

public class TransactionServices : ITransactionServices
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public TransactionServices(ITransactionRepository transactionRepository, IUserRepository userRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
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
        
        var userBalance = await _userRepository.GetUserBalanceAsync(user_id);
        if (type == "deposit")
        {
            userBalance += nominal;
        }
        else
        {
            userBalance -= nominal;
        }

        try
        {
            await _transactionRepository.AddTransaction(transaction);
            await _userRepository.UpdateUserBalanceAsync(user_id, userBalance);
            return true;
        }
        catch (Exception ex)
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