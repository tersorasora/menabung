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
public class TransactionController : ControllerBase
{
    private readonly ITransactionServices _transactionServices;

    public TransactionController(ITransactionServices transactionServices)
    {
        _transactionServices = transactionServices;
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction request)
    {
        var userIdToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        Console.Error.WriteLine("User ID from token: " + userIdToken);
        var result = await _transactionServices.AddTransactions(request.description, request.transaction_type, request.transaction_nominal, userIdToken);
        if (!result)
        {
            return BadRequest("Failed to add transaction.");
        }
        return Ok("Transaction added successfully.");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetTransactionsByUserID(int userId)
    {
        var transaction = await _transactionServices.GetTransactionsByUserIdAsync(userId);
        if (transaction == null || transaction.Count == 0) {
            return NotFound("No transactions found for this user.");
        }
        return Ok(transaction);
    }
}