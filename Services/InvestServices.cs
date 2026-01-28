using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;

namespace Services;

public class InvestServices : IInvestServices
{
    private readonly IInvestRepository _investRepository;
    private readonly IUserRepository _userRepository;

    public InvestServices(IInvestRepository investRepository, IUserRepository userRepository)
    {
        _investRepository = investRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> AddInvest(string invest_type, decimal quantity, string quantity_type, decimal price, DateTime date, bool is_sell, int user_id)
    {
        var invest = new Invests
        {
            invest_type = invest_type,
            quantity = quantity,
            quantity_type = quantity_type,
            price = price,
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc),
            is_sell = is_sell,
            user_id = user_id
        };

        var userBalance = await _userRepository.GetUserBalanceAsync(user_id);
        if (is_sell)
        {
            userBalance += price * quantity;
        }
        else
        {
            userBalance -= price * quantity;   
        }

        try
        {
            await _investRepository.AddInvest(invest);
            await _userRepository.UpdateUserBalanceAsync(user_id, userBalance);
            return true;
        }
        catch (DbUpdateException dbEx)
        {
            // This gets the real inner exception from PostgreSQL
            var sqlEx = dbEx.InnerException;
            Console.WriteLine("DbUpdateException occurred:");
            Console.WriteLine("Message: " + dbEx.Message);
            if (sqlEx != null)
            {
                Console.WriteLine("Inner Exception: " + sqlEx.Message);
                Console.WriteLine("Full Inner Exception: " + sqlEx.ToString());
            }
            throw; // or return a BadRequest with details while testing
        }
        catch (Exception ex)
        {
            Console.WriteLine("Other Exception: " + ex.ToString());
            throw;
        }
    }
    
    public Task<List<Invests>> GetAllInvestsAsync()
    {
        return _investRepository.GetAllInvestsAsync();
    }

    public Task<List<Invests>> GetInvestsByUserIdAsync(int userId)
    {
        return _investRepository.GetInvestByUserID(userId);
    }

    public async Task<decimal> CountAssetByTypeAsync(string investType, int userId)
    {
        List<Invests> invests = await _investRepository.GetInvestByType(investType, userId);
        decimal result = 0;
        foreach(var i in invests)
        {
            if (i.is_sell)
            {
                result -= i.quantity;
            }
            else
            {
                result += i.quantity;
            }
        }
        return result;
    }

    public async Task<int> CountUserProfits(int userId)
    {
        List<Invests> invests = await _investRepository.GetInvestByUserID(userId);
        int buyTotal = 0;
        int sellTotal = 0;

        foreach(var i in invests)
        {
           if(i.is_sell)
           {
               sellTotal += (int)(i.price * i.quantity);
            }
            else
            {
                buyTotal += (int)(i.price * i.quantity);
            }
        }

        if(sellTotal == 0) // No sells yet
        {
            return 0;
        }

        return sellTotal - buyTotal;
    }

    public async Task<bool> EditInvest(int investId, string invest_type, decimal quantity, string quantity_type, decimal price, DateTime date)
    {
        var invest = await _investRepository.GetInvestByIdAsync(investId);
        if (invest == null)
        {
            return false;
        }

        var userBalance = await _userRepository.GetUserBalanceAsync(invest.user_id);

        if(invest.price != price && price != 0)
        {
            if (invest.is_sell)
            {
                if(price > invest.price)
                {
                    userBalance += (price - invest.price) * quantity;
                }
                else
                {
                    userBalance -= (invest.price - price) * quantity;
                }
            }
            else
            {
                if(price > invest.price)
                {
                    userBalance -= (price - invest.price) * quantity;
                }
                else
                {
                    userBalance += (invest.price - price) * quantity;
                }
            }
        }

        invest.invest_type = invest_type;
        invest.quantity = quantity;
        invest.quantity_type = quantity_type;
        invest.price = price;
        invest.date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        try
        {
            await _investRepository.EditInvest(invest);
            await _userRepository.UpdateUserBalanceAsync(invest.user_id, userBalance);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing Invest: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteInvest(int investId)
    {
        var invest = await _investRepository.GetInvestByIdAsync(investId);
        if (invest == null)
        {
            return false;
        }

        var userBalance = await _userRepository.GetUserBalanceAsync(invest.user_id);

        // Revert the transaction effect
        if (invest.is_sell)
        {
            userBalance -= invest.price * invest.quantity;
        }
        else
        {
            userBalance += invest.price * invest.quantity;
        }

        try
        {
            await _investRepository.DeleteInvest(investId);
            await _userRepository.UpdateUserBalanceAsync(invest.user_id, userBalance);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting Invest: {ex.Message}");
            return false;
        }
    }
}