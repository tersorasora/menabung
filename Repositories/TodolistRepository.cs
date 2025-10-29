using Models;
using data;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class ToDoListRepository : IToDoListRepository
{
    private readonly AppDBContext _context;
    public ToDoListRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddToDoList(ToDoList toDoList)
    {
        _context.toDoList.Add(toDoList);
        await _context.SaveChangesAsync();
    }

    public async Task<ToDoList?> GetToDoListByIdAsync(int id)
    {
        return await _context.toDoList.FindAsync(id);
    }

    public async Task<List<ToDoList>> GetAllToDoListsAsync()
    {
        return await _context.toDoList
            .OrderBy(t => t.todolist_id)
            .Include(t => t.user)
            .ToListAsync();
    }

    public async Task<List<ToDoList>> GetAllToDoListsByUserIdAsync(int? userId)
    {
        return await _context.toDoList
            .Include(t => t.user)
            .Where(t => t.user_id == userId)
            .OrderBy(t => t.todolist_id)
            .ToListAsync();
    }

    public async Task EditToDoList(ToDoList toDoList)
    {
        _context.toDoList.Update(toDoList);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteToDoListAsync(int id)
    {
        var toDoList = await _context.toDoList.FindAsync(id);
        if (toDoList != null)
        {
            _context.toDoList.Remove(toDoList);
            await _context.SaveChangesAsync();
        }
    }
}