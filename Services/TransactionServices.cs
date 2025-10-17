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

    public async Task<bool> AddTransactions(string description, string type, decimal nominal, DateTime date, int user_id)
    {
        var transaction = new Transaction
        {
            description = description,
            transaction_type = type,
            transaction_nominal = nominal,
            user_id = user_id,
            transaction_date = date
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
    
    public Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return _transactionRepository.GetAllTransactionsAsync();
    }

    public Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
    {
        return _transactionRepository.GetTransactionsByUserIdAsync(userId);
    }

    public async Task<bool> EditTransaction(int transactionId, string description, string type, decimal nominal)
    {
        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(transactionId);
        var transaction = transactions.FirstOrDefault(t => t.transaction_id == transactionId);
        if (transaction == null)
        {
            return false;
        }

        var userBalance = await _userRepository.GetUserBalanceAsync(transaction.user_id);

        // Revert the old transaction effect
        if (transaction.transaction_type == "deposit")
        {
            userBalance -= transaction.transaction_nominal;
        }
        else
        {
            userBalance += transaction.transaction_nominal;
        }

        // Apply the new transaction effect
        if (type == "deposit")
        {
            userBalance += nominal;
        }
        else
        {
            userBalance -= nominal;
        }

        transaction.description = description;
        transaction.transaction_type = type;
        transaction.transaction_nominal = nominal;

        try
        {
            await _transactionRepository.EditTransaction(transaction);
            await _userRepository.UpdateUserBalanceAsync(transaction.user_id, userBalance);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing transaction: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteTransaction(int transactionId)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
        if (transaction == null)
        {
            Console.WriteLine("Null mas e");
            return false;
        }

        Console.WriteLine("Ora Null mas e");

        var userBalance = await _userRepository.GetUserBalanceAsync(transaction.user_id);
        Console.WriteLine($"Current user id : {transaction.user_id}");

        // Revert the transaction effect
        if (transaction.transaction_type == "deposit")
        {
            userBalance -= transaction.transaction_nominal;
        }
        else
        {
            userBalance += transaction.transaction_nominal;
        }

        try
        {
            await _transactionRepository.DeleteTransaction(transactionId);
            await _userRepository.UpdateUserBalanceAsync(transaction.user_id, userBalance);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting transaction: {ex.Message}");
            return false;
        }
    }
}