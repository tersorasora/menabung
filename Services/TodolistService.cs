using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using System.Text;

namespace Services;

public class ToDoListService : IToDoListService
{
    private readonly IToDoListRepository _toDoListRepository;
    private readonly IUserRepository _userRepository;
    public ToDoListService(IToDoListRepository toDoListRepository, IUserRepository userRepository)
    {
        _toDoListRepository = toDoListRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> CreateToDoListAsync(string objective, int userId)
    {
        var toDoList = new ToDoList
        {
            objective = objective,
            todolist_status = false,
            todolist_date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            user_id = userId
        };

        try
        {
            await _toDoListRepository.AddToDoList(toDoList);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred while creating ToDoList: " + ex.ToString());
            return false;
        }
    }

    public async Task<ToDoList?> GetToDoListByIdAsync(int id)
    {
        return await _toDoListRepository.GetToDoListByIdAsync(id);
    }

    public async Task<List<ToDoList>> GetAllToDoListsAsync(int? userId)
    {
        if (userId.HasValue)
        {
            return await _toDoListRepository.GetAllToDoListsByUserIdAsync(userId);
        }
        return await _toDoListRepository.GetAllToDoListsAsync();
    }

    public async Task<bool> UpdateToDoListAsync(int tdlID, string objective, bool isCompleted)
    {
        var toDoList = await _toDoListRepository.GetToDoListByIdAsync(tdlID);
        if (toDoList == null)
        {
            Console.WriteLine("ToDoList not found with ID: " + tdlID);
            return false;
        }

        if (!string.IsNullOrEmpty(objective))
        {
            toDoList.objective = objective;
        }

        if (isCompleted != toDoList.todolist_status)
        {
            toDoList.todolist_status = isCompleted;
        }

        try
        {
            await _toDoListRepository.EditToDoList(toDoList);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred while updating ToDoList: " + ex.ToString());
            return false;
        }
    }
    
    public async Task<bool> DeleteToDoListAsync(int id)
    {
        try
        {
            await _toDoListRepository.DeleteToDoListAsync(id);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred while deleting ToDoList: " + ex.ToString());
            return false;
        }
    }
}