using Microsoft.AspNetCore.Mvc;
using Models;
using data;
using Microsoft.AspNetCore.Identity;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleNameByID(int id)
    {
        var roleName = await _roleService.GetRoleNameByIdAsync(id);
        if (roleName == null)
        {
            return NotFound("Role not found.");
        }
        return Ok(new { role_name = roleName });
    }
}