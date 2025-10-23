using Models;

namespace Services;

public interface IRoleService
{
    Task<string?> GetRoleNameByIdAsync(int roleId);
}