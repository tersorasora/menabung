using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodolistController : ControllerBase
{
    private readonly IToDoListService _toDoListService;

    public TodolistController(IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateToDoList([FromBody] ToDoList request)
    {
        var userIdToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _toDoListService.CreateToDoListAsync(request.objective, userIdToken);
        if (!result)
        {
            return BadRequest("Failed to create to-do list.");
        }
        return Ok("To-do list created successfully.");
    }

    [HttpGet("byid/{id}")]
    public async Task<IActionResult> GetToDoListById(int id)
    {
        var toDoList = await _toDoListService.GetToDoListByIdAsync(id);
        if (toDoList == null)
        {
            return NotFound("To-do list not found.");
        }
        return Ok(toDoList);
    }

    [HttpGet("GetAllToDoLists/{userId?}")]
    public async Task<IActionResult> GetAllToDoLists(int? userId)
    {
        var toDoLists = await _toDoListService.GetAllToDoListsAsync(userId);
        if (toDoLists == null || !toDoLists.Any())
        {
            return NotFound("No to-do lists found.");
        }
        return Ok(new { todolists = toDoLists });
    }

    [Authorize]
    [HttpPut("update/{tdlID}")]
    public async Task<IActionResult> UpdateToDoList(int tdlID, string objective, bool isCompleted)
    {
        var result = await _toDoListService.UpdateToDoListAsync(tdlID, objective, isCompleted);
        if (!result)
        {
            return BadRequest("Failed to update to-do list.");
        }
        return Ok("To-do list updated successfully.");
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteToDoList(int id)
    {
        var result = await _toDoListService.DeleteToDoListAsync(id);
        if (!result)
        {
            return BadRequest("Failed to delete to-do list.");
        }
        return Ok("To-do list deleted successfully.");
    }
}