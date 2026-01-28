using Models;
using data;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class InvestRepository : IInvestRepository
{
    private readonly AppDBContext _context;

    public InvestRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddInvest(Invests invest)
    {
        _context.invests.Add(invest);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Invests>> GetAllInvestsAsync()
    {
        return await _context.invests.OrderBy(i => i.invest_id)
            .ToListAsync();
    }

    public async Task<List<Invests>> GetInvestByUserID(int userID)
    {
        return await _context.invests.Include(i => i.user)
            .Where(i => i.user_id == userID)
            .OrderBy(i => i.invest_id)
            .ToListAsync();
    }

    public async Task<List<Invests>> GetInvestByType(string investType, int userID)
    {
        return await _context.invests.Include(i => i.user)
            .Where(i => i.invest_type == investType && i.user_id == userID)
            .OrderBy(i => i.invest_id)
            .ToListAsync();
    }

    public async Task<Invests?> GetInvestByIdAsync(int investId)
    {
        return await _context.invests.FindAsync(investId);
    }

    public async Task EditInvest(Invests invest)
    {
        _context.invests.Update(invest);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteInvest(int investId)
    {
        var invest = await _context.invests.FindAsync(investId);
        if (invest != null)
        {
            _context.invests.Remove(invest);
            await _context.SaveChangesAsync();
        }
    }
}