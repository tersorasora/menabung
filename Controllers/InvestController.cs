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
public class InvestController : ControllerBase
{
    private readonly IInvestServices _investServices;

    public InvestController(IInvestServices investServices)
    {
        _investServices = investServices;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddInvest([FromBody] Invests request)
    {
        var userIdToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _investServices.AddInvest(request.invest_type, request.quantity, request.quantity_type, request.price, request.date, request.is_sell, userIdToken);
        if (!result)
        {   
            return BadRequest("Failed to add invest.");
        }
        return Ok("Invest added successfully.");
    }

    [HttpGet("GetAllInvests")]
    public async Task<IActionResult> GetAllInvests()
    {
        var invests = await _investServices.GetAllInvestsAsync();
        if (invests == null || invests.Count == 0)
        {
            return NotFound("No invests found.");
        }
        return Ok(invests);
    }

    [HttpGet("user/{user_id}")]
    public async Task<IActionResult> GetInvestsByUserID(int user_id)
    {
        var invests = await _investServices.GetInvestsByUserIdAsync(user_id);
        if (invests == null || invests.Count == 0)
        {
            return NotFound("No invests found for this user.");
        }
        return Ok(new { invests });
    }

    [HttpGet("count/{investType}/{userId:int}")]
    public async Task<IActionResult> CountAssetByType(string investType, int userId)
    {
        var count = await _investServices.CountAssetByTypeAsync(investType, userId);
        return Ok(new { count = count });
    }

    [HttpGet("profits")]
    public async Task<IActionResult> GetUserProfits()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var profits = await _investServices.CountUserProfits(userId);
        return Ok(new { profits = profits });
    }

    [Authorize]
    [HttpPut("edit/{investId}")]
    public async Task<IActionResult> EditInvest(int investId, [FromBody] Invests request)
    {
        var result = await _investServices.EditInvest(investId, request.invest_type, request.quantity, request.quantity_type, request.price, request.date);
        if (!result)
        {
            return BadRequest("Failed to edit invest.");
        }
        return Ok("Invest edited successfully.");
    }

    [Authorize]
    [HttpDelete("delete/{investId}")]
    public async Task<IActionResult> DeleteInvest(int investId)
    {
        var result = await _investServices.DeleteInvest(investId);
        if (!result)
        {
            return BadRequest("Failed to delete invest.");
        }
        return Ok("Invest deleted successfully.");
    }
}