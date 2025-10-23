using Models;
using System.Text;

namespace Repositories;

public interface IRoleRepository
{
    Task<string?> GetRoleNameByIdAsync(int roleId);
}