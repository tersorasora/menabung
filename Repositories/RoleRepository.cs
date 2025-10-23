using Models;
using data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDBContext _context;

    public RoleRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<string?> GetRoleNameByIdAsync(int roleId)
    {
        var role = await _context.roles.FindAsync(roleId);
        return role?.role_name;
    }
}