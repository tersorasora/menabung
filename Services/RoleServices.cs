using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using System.Text;

namespace Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<string?> GetRoleNameByIdAsync(int roleId)
    {
        return await _roleRepository.GetRoleNameByIdAsync(roleId);
    }
}