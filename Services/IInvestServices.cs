using System.ComponentModel;
using Models;

namespace Services;

public interface IInvestServices
{
    Task<bool> AddInvest(string invest_type, decimal quantity, string quantity_type, decimal price, DateTime date, bool is_sell, int user_id);
    Task<List<Invests>> GetAllInvestsAsync();
    Task<List<Invests>> GetInvestsByUserIdAsync(int userId);
    Task<decimal> CountAssetByTypeAsync(string investType, int userId);
    Task<int> CountUserProfits(int userId);
    Task<bool> EditInvest(int investId, string invest_type, decimal quantity, string quantity_type, decimal price, DateTime date);
    Task<bool> DeleteInvest(int investId);
}