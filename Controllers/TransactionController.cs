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
        var result = await _transactionServices.AddTransactions(request.description, request.transaction_type, request.transaction_nominal, request.transaction_date, userIdToken);
        if (!result)
        {
            return BadRequest("Failed to add transaction.");
        }
        return Ok("Transaction added successfully.");
    }

    [HttpGet("GetAllTransactions")]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _transactionServices.GetAllTransactionsAsync();
        if (transactions == null || transactions.Count == 0)
        {
            return NotFound("No transactions found.");
        }
        return Ok(transactions);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetTransactionsByUserID()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var transaction = await _transactionServices.GetTransactionsByUserIdAsync(userId);
        if (transaction == null || transaction.Count == 0)
        {
            return NotFound("No transactions found for this user.");
        }
        return Ok(transaction);
    }

    [HttpGet("byid/{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var transaction = await _transactionServices.GetTransactionsByUserIdAsync(id);
        if (transaction == null || !transaction.Any())
        {
            return NotFound(new { Message = "Transaction not found." });
        }
        return Ok(new { transaction });
    }

    [HttpGet("CountUserTransactions/{userId}")]
    public async Task<IActionResult> CountUserTransactions(int userId)
    {
        var count = await _transactionServices.CountUserTransactionsAsync(userId);
        return Ok(new { total_transactions = count });
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> EditTransaction(int id, [FromBody] Transaction request)
    {
        var result = await _transactionServices.EditTransaction(id, request.description, request.transaction_type, request.transaction_nominal);
        if (!result)
        {
            return BadRequest("Failed to edit transaction.");
        }
        return Ok("Transaction edited successfully.");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var result = await _transactionServices.DeleteTransaction(id);
        if (!result)
        {
            return BadRequest("Failed to delete transaction.");
        }
        return Ok("Transaction deleted successfully.");
    }
}