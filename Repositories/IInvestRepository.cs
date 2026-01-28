using Models;

namespace Repositories;

public interface IInvestRepository
{
    Task AddInvest(Invests invest);
    Task<List<Invests>> GetAllInvestsAsync();
    Task<List<Invests>> GetInvestByUserID(int userID);
    Task<List<Invests>> GetInvestByType(string investType, int userID);
    Task<Invests?> GetInvestByIdAsync(int investId);
    Task EditInvest(Invests invest);
    Task DeleteInvest(int investId);
}