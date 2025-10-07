using Microsoft.AspNetCore.Mvc;
using Models;
using data;
using Microsoft.AspNetCore.Identity;
using Services;

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

    [HttpPost("add")]
    public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
    {
        var result = await _transactionServices.AddTransactions(transaction.transaction_type, transaction.transaction_nominal, transaction.user_id);
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